using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace LazormPageGenerator
{
        public class PageGenerator
    {
        public static void GenerateFromFile(string filePath, string outDir)
        {
            // ソースコードをテキストとして読み込む
            var code = File.ReadAllText(filePath);
            
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            // 構文木の生成

            var syntaxTree = CSharpSyntaxTree.ParseText(code, CSharpParseOptions.Default, filePath);
            // 構文木をリストへ格納
            syntaxTrees.Add(syntaxTree);

            var compilation = CSharpCompilation.Create("lazorm", syntaxTrees, null);

            var pluralizer = new Pluralize.NET.Pluralizer();
            GeneratorContext context = new GeneratorContext();

            Trace.WriteLine("Parsing source file...");
            #region Source File Parsing
            foreach (var tree in syntaxTrees)
            {
                // コンパイラからセマンティックモデルの取得
                var semanticModel = compilation.GetSemanticModel(tree);
                // 構文木からルートの子ノード群を取得
                var nodes = tree.GetRoot().DescendantNodes();

                // ノード群からクラスに関する構文情報群を取得
                // クラスはClassDeclarationSyntax
                // インタフェースはInterfaceDeclarationSyntax
                // 列挙型はEnumDeclarationSyntax
                // 構造体はStructDeclarationSyntax
                // デリゲートはDelegateDeclarationSyntax
                var classSyntaxArray = nodes.OfType<ClassDeclarationSyntax>();
                foreach (var syntax in classSyntaxArray)
                {
                    var symbol = semanticModel.GetDeclaredSymbol(syntax);
                    Trace.WriteLine("{symbol!.DeclaredAccessibility} {symbol}");
                    Trace.WriteLine(" {nameof(symbol.IsAbstract)}: {symbol.IsAbstract}");
                    Trace.WriteLine(" {nameof(symbol.IsStatic)}: {symbol.IsStatic}" );
                    if (symbol!.Name == Path.GetFileNameWithoutExtension(filePath))
                    {
                        context.EntityClassName = symbol.Name.Capitalize();
                        context.EntityClassNamePlural = pluralizer.Pluralize(symbol.Name).Capitalize();
                    }

                    //// 継承しているクラスやインタフェースがあるかどうか
                    //if (syntax.BaseList != null)
                    //{
                    //    // 継承しているクラスなどのシンボルを取得
                    //    var inheritanceList = from baseSyntax in syntax.BaseList.Types
                    //                          let symbolInfo = semanticModel.GetSymbolInfo(baseSyntax.Type)
                    //                          let sym = symbolInfo.Symbol
                    //                          select sym;

                    //    // 継承しているクラスなどを出力
                    //    Console.WriteLine(" Inheritance:");
                    //    foreach (var inheritance in inheritanceList)
                    //        Console.WriteLine("  {0}", inheritance.ToDisplayString());
                    //}
                }

                var propertySyntaxArray = nodes.OfType<PropertyDeclarationSyntax>();
                var pageColumns = new List<System.Reflection.PropertyInfo>();
                foreach (var syntax in propertySyntaxArray)
                {
                    var symbol = semanticModel.GetDeclaredSymbol(syntax)!;
                    Trace.WriteLine($"{symbol.DeclaredAccessibility} {symbol}");
                    Trace.WriteLine($" Namespace: {symbol.ContainingSymbol}" );
                    Trace.WriteLine($" Class: {symbol.ContainingType.Name}" );
                    Trace.WriteLine($" {nameof(symbol.IsStatic)}: {symbol.IsStatic}");

                    // アクセサの取得
                    var accessors = from accessor in syntax.AccessorList!.Accessors
                                    select new
                                    {
                                        Name = accessor.Keyword.ToString(),
                                        Access = accessor.Modifiers.Count > 0 ?
                                            semanticModel.GetDeclaredSymbol(accessor)!.DeclaredAccessibility :
                                            Accessibility.Public
                                    };

                    // クエリ式使わない場合
                    //var accessors = new List<(string Name, Accessibility Access)>();
                    //foreach (var accessor in syntax.AccessorList.Accessors)
                    //{
                    //    var accessibility = Accessibility.Public;
                    //    var keyword = accessor.Keyword.ToString();
                    //    if (accessor.Modifiers.Count > 0)
                    //    {
                    //        var msym = semanticModel.GetDeclaredSymbol(accessor);
                    //        accessibility = msym.DeclaredAccessibility;
                    //    }
                    //    accessors.Add((keyword, accessibility));
                    //}

                    // アクセサの出力
                    Trace.WriteLine(" Accessors:");
                    foreach (var accessor in accessors)
                        Trace.WriteLine($"  {accessor.Access} {accessor.Name}");

                    // 戻り値の出力
                    Trace.WriteLine($" Type: {symbol.Type}");
                    if(symbol.ContainingType.Name == Path.GetFileNameWithoutExtension(filePath))
                    {
                        context.EntityProperties.Add(new EntityProperty(symbol.Name, accessors.Any(p => p.Name == "set"), symbol.Type.ToString() ?? string.Empty));
                    }
                }
            }
            #endregion
            Trace.WriteLine("Source file parsing end");

            //    #region ListPage

            // creating list page file
            ListPageTemplate listTemplate = new(context: context);
            string listReducerPageContent = listTemplate.TransformText();
            var dir = Directory.CreateDirectory(path: Path.Combine(outDir, context.EntityClassNamePlural));
            File.WriteAllText(Path.Combine(dir.FullName, "Index.razor"), listReducerPageContent);

            // creating confirmation dialog file
            ConfirmDialogTemplate confirmDialogTemplate = new ConfirmDialogTemplate();

            // creating detail page file
            DetailPageTemplate detailPageTemplate = new(context: context);
            string detailPageContent = detailPageTemplate.TransformText();
            File.WriteAllText(Path.Combine(dir.FullName, $"Edit{context.EntityClassName}.razor"), detailPageContent);

            // creating detail page with fluxor
            DetailPageWithFluxorTemplate detailPageWithFluxor = new DetailPageWithFluxorTemplate(context: context);
            string detailPageWithFluxorContent = detailPageWithFluxor.TransformText();
            File.WriteAllText(Path.Combine(dir.FullName, $"Edit{context.EntityClassName}WithFluxor.razor"), detailPageWithFluxorContent);
            DetailPageWithFluxorCsTemplate fluxorCs = new DetailPageWithFluxorCsTemplate(context: context);
            string fluxorCsContent = fluxorCs.TransformText();
            File.WriteAllText(Path.Combine(dir.FullName, $"Edit{context.EntityClassName}WithFluxor.razor.cs"), fluxorCsContent);


        }

        //public static void Generate(T entity, string outDir)
        //{
        //    if (entity is null)
        //        throw new ArgumentException("Entity Name must be specified.");
        //    if (string.IsNullOrWhiteSpace(outDir))
        //        outDir = Environment.CurrentDirectory;
        //    Environment.CurrentDirectory = outDir;


        //    var pluralizer = new Pluralize.NET.Pluralizer();
        //    GeneratorContext context = new GeneratorContext()
        //    {
        //        EntityClassName = entity.GetType().Name
        //    };
        //    context.PluralName = pluralizer.Pluralize(context.EntityClassName);

        //    #region ListPage
        //    ListPageTemplate listTemplate = new ListPageTemplate(context: context);
        //    string listReducerPageContent = listTemplate.TransformText();
        //    Directory.CreateDirectory($"Pages/{context.PluralName}/");
        //    File.WriteAllText($"Pages/{context.PluralName}/Index.cs", listReducerPageContent);
        //    #endregion

        //    // Detail Page Templating


        //}
    }
}