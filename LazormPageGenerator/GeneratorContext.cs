using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LazormPageGenerator
{
    public class GeneratorContext
    {
        public GeneratorContext(string filePath)
        {
            // ソースコードをテキストとして読み込む
            var code = File.ReadAllText(filePath);

            // 構文木の生成
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            var syntaxTree = CSharpSyntaxTree.ParseText(code, CSharpParseOptions.Default, filePath);
            // 構文木をリストへ格納
            syntaxTrees.Add(syntaxTree);

            var compilation = CSharpCompilation.Create("lazorm", syntaxTrees, null);

            var pluralizer = new Pluralize.NET.Pluralizer();

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
                    Trace.WriteLine(" {nameof(symbol.IsStatic)}: {symbol.IsStatic}");
                    if (symbol!.Name == Path.GetFileNameWithoutExtension(filePath))
                    {
                        this.EntityClassName = symbol.Name.ToPascalCase();
                        this.EntityClassNamePlural = pluralizer.Pluralize(symbol.Name).ToPascalCase();
                    }
                }

                var recordSyntaxArray = nodes.OfType<RecordDeclarationSyntax>();
                foreach (var syntax in recordSyntaxArray)
                {
                    var symbol = semanticModel.GetDeclaredSymbol(syntax);
                    Trace.WriteLine("{symbol!.DeclaredAccessibility} {symbol}");
                    Trace.WriteLine(" {nameof(symbol.IsAbstract)}: {symbol.IsAbstract}");
                    Trace.WriteLine(" {nameof(symbol.IsStatic)}: {symbol.IsStatic}");
                    if (symbol!.Name == Path.GetFileNameWithoutExtension(filePath))
                    {
                        this.EntityClassName = symbol.Name.ToPascalCase();
                        this.EntityClassNamePlural = pluralizer.Pluralize(symbol.Name).ToPascalCase();
                    }
                }

                var structSyntaxArray = nodes.OfType<StructDeclarationSyntax>();
                foreach (var syntax in structSyntaxArray)
                {
                    var symbol = semanticModel.GetDeclaredSymbol(syntax);
                    Trace.WriteLine("{symbol!.DeclaredAccessibility} {symbol}");
                    Trace.WriteLine(" {nameof(symbol.IsAbstract)}: {symbol.IsAbstract}");
                    Trace.WriteLine(" {nameof(symbol.IsStatic)}: {symbol.IsStatic}");
                    if (symbol!.Name == Path.GetFileNameWithoutExtension(filePath))
                    {
                        this.EntityClassName = symbol.Name.ToPascalCase();
                        this.EntityClassNamePlural = pluralizer.Pluralize(symbol.Name).ToPascalCase();
                    }
                }

                if (string.IsNullOrWhiteSpace(this.EntityClassName))
                    throw new ArgumentException("couldn't find class or record or struct name");

                var propertySyntaxArray = nodes.OfType<PropertyDeclarationSyntax>();
                var pageColumns = new List<System.Reflection.PropertyInfo>();
                foreach (var syntax in propertySyntaxArray.Where(p => p.Modifiers.Any(SyntaxKind.PublicKeyword)))
                {
                    var symbol = semanticModel.GetDeclaredSymbol(syntax)!;
                    Trace.WriteLine($"{symbol.DeclaredAccessibility} {symbol}");
                    Trace.WriteLine($" Namespace: {symbol.ContainingSymbol}");
                    Trace.WriteLine($" Class: {symbol.ContainingType.Name}");
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
                    if (symbol.ContainingType.Name == Path.GetFileNameWithoutExtension(filePath))
                    {
                        this.EntityProperties.Add(new EntityProperty(symbol.Name, accessors.Any(p => p.Name == "set"), symbol.Type.ToString() ?? string.Empty));
                    }
                }
            }
            #endregion
            Trace.WriteLine("Source file parsing end");
            return;
        }
        public string EntityClassName { get; set; } = string.Empty;

        public string EntityClassNamePlural { get; set; } = string.Empty;

        public string EntityName { get { return this.EntityClassName.Uncapitalize(); } } 

        public string EntityNamePlural { get { return this.EntityClassNamePlural.Uncapitalize(); } }

        public List<EntityProperty> EntityProperties { get; set; }  = new List<EntityProperty>();

        public string Namespace { get; set; } = "Lazorm";
    }

    public class EntityProperty
    {
        public EntityProperty(string name, bool editable, string returnTypeName)
        {
            this.Name = name;
            this.Editable = editable;
            this.TypeName = returnTypeName;
        }
        public string Name { get; }
        public bool Editable { get; }
        public string TypeName { get; }
    }
}
