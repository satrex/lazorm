using System;
using Microsoft.Extensions.CommandLineUtils;

namespace Lazorm 
{
    class Program
    {
        static void Main(string[] args)
        {
            Array.ForEach(args, a => Console.WriteLine(a)); 
            var app = new CommandLineApplication(throwOnUnexpectedArg: true);

            app.Name = nameof(Lazorm);
            app.Description = @"データベースから、エンティティクラスを生成します。";

            app.HelpOption("-h|--help");

            var dbKindArgument = app.Argument(
                name: "kind",
                description: "接続先データベースの製品名を指定します。対応データベース: sqlServer, oracle, mySql",
                multipleValues: false);

            var constrArgument = app.Argument(
                name: "connectionString",
                description: "接続文字列",
                multipleValues: false);

            var tablesArgument = app.Argument(
                name: "tables",
                description: "テーブル名を列挙します。",
                multipleValues: true);

            var outFolderOption = app.Option(
                template: "-o|--out",
                description: "出力先フォルダを指定します。",
                optionType: CommandOptionType.MultipleValue);

            var listOption = app.Option(
                template: "-l|--ls|--list",
                description: "テーブル一覧を取得します。",
                optionType: CommandOptionType.NoValue);

            app.OnExecute(() =>
            {
                if (constrArgument.Value == null)
                {
                    // Console.WriteLine("constr: {0} tables:{1} ", constrArgument.Value, tablesArgument.Value);

                    app.ShowHelp();
                    Console.WriteLine(@"使用例：
    dotnet lazorm -- sqlServer 'initial catalog=...' -ls 
    -> テーブル一覧を表示します。

    dotnet lazorm -- sqlServer 'initial catalog=...' users,logs
    -> users,logsテーブルのエンティティクラスをカレントディレクトリに生成します。
    ");
                    return 1;
                }

                Database db = Database.CreateInstance(dbKindArgument.Value, constrArgument.Value);

                if(listOption.HasValue()){
                    Console.WriteLine("===  TABLE NAME  ===\n--------------------");
                    db.GetTableDefs().ForEach(t => 
                        Console.WriteLine("{0} ", t.Name)
                    );
                    return 0;
                }

                var outFolder = Environment.CurrentDirectory;
                if (outFolderOption.HasValue())
                {
                    outFolder = outFolderOption.Value();
                }

                // string server = builder.DataSource;
                var generator = new DataEntityGenerator("DataCore", db, string.Format("{0}Key", db.Schema));
                var tableArgsTemp = tablesArgument.Value.Replace(",", " ");
                var regex = new System.Text.RegularExpressions.Regex(" +").Replace(tableArgsTemp, " ");
                var tableArgs = regex.Split(" ");
                Array.ForEach(tableArgs, t => {
                    generator.Generate(t, outFolder);
                });
                return 0;
            });

            app.Execute(args);
        }
    }
}
