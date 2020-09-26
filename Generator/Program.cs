﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;

namespace Lazorm 
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly thisAssem = typeof(Program).Assembly;
            AssemblyName thisAssemName = thisAssem.GetName();
            Version ver = thisAssemName.Version;

            Console.WriteLine("Lazorm (c)satrex");
            Console.WriteLine("assembly {1} version {0}", ver, thisAssemName.Name);
            Console.WriteLine();

            Array.ForEach(args, a => Debug.WriteLine(a)); 
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
            var app = new CommandLineApplication(throwOnUnexpectedArg: true);

            app.Name = nameof(Lazorm);
            app.Description = @"generates entitiy class files from database schema. \nデータベースから、エンティティクラスを生成します。";

            app.HelpOption("-h|--help");

            var dbKindArgument = app.Argument(
                name: "kind",
                description: "接続先データベースの製品名を指定します。対応データベース: sqlServer, oracle, mySql",
                multipleValues: false);

            var constrArgument = app.Argument(
                name: "connectionString",
                description: "接続文字列",
                multipleValues: false);

            var tablesOption = app.Option(
                template: "-t|--tables",
                description: "テーブル名を列挙します。",
                optionType: CommandOptionType.MultipleValue);

            var outFolderOption = app.Option(
                template: "-o|--out",
                description: "出力先フォルダを指定します。",
                optionType: CommandOptionType.MultipleValue);

            var settingsJsonOption = app.Option(
                template: "-j|--json",
                description: "appsetteings.jsonファイルを指定します。",
                optionType: CommandOptionType.SingleValue);

            var showTablesOption = app.Option(
                template: "-l|--ls|--list",
                description: "テーブル一覧を表示します。",
                optionType: CommandOptionType.NoValue);

            app.OnExecute(() =>
            {
                if (constrArgument.Value == null)
                {
                    // Console.WriteLine("constr: {0} tables:{1} ", constrArgument.Value, tablesArgument.Value);

                    app.ShowHelp();
                    Console.WriteLine(@"
                    example:
                    
    dotnet lazorm -- sqlServer 'initial catalog=...' -ls 
    -> shows list of tables in the schema.

    dotnet lazorm -- sqlServer 'initial catalog=...'
    -> generates entity class files of all tables of schema.

    dotnet lazorm -- sqlServer 'initial catalog=...' -t users,logs
    -> generates class files of users,logs tables.
                    -----------------------------------------------------
                    使用例：
    dotnet lazorm -- sqlServer 'initial catalog=...' -ls 
    -> テーブル一覧を表示します。

    dotnet lazorm -- sqlServer 'initial catalog=...'
    -> スキーマの全テーブルをエンティティクラスとして生成します。

    dotnet lazorm -- sqlServer 'initial catalog=...' -t users,logs
    -> users,logsテーブルのエンティティクラスをEntities/AutoGeneratedフォルダに生成します。
    ");
                    return 1;
                }

                Database db = Database.CreateInstance(dbKindArgument.Value, constrArgument.Value);

                if(showTablesOption.HasValue()){
                    Console.WriteLine("===  TABLE NAME  ===\n--------------------");
                    db.GetTableDefs().ForEach(t => 
                        Console.WriteLine("{0} ", t.Name)
                    );
                    return 0;
                }

                var outFolder = Path.Combine(Environment.CurrentDirectory, "Entities", "autoGenerated");
                if (outFolderOption.HasValue())
                {
                    outFolder = outFolderOption.Value();
                    Console.WriteLine("Output directory is specified: {0}", outFolder );
                }

                // Creates folder if not exists
                if (!Directory.Exists(outFolder))
                {
                    Directory.CreateDirectory(outFolder);
                }

                // // Throws error if outFolder is not empty
                // if (0 < Directory.GetFiles(outFolder).Length )
                // {
                //     throw new ApplicationException("出力先フォルダが空ではありません。");
                // }
                
                var keyName = string.Format("{0}Key", db.Schema);
                var generator = new DataEntityGenerator("Lazorm", db, keyName);
                var tables = new List<string>();

                // if tables not specified, then go all tables
                if(tablesOption.HasValue()){
                    var tableArgsTemp = tablesOption.Value().Replace(",", " ");
                    var regex = new System.Text.RegularExpressions.Regex(" +").Replace(tableArgsTemp, " ");
                    tables.AddRange(regex.Split(" "));
                    //tables.AddRange(tablesOption.Values);
                }
                else 
                {
                    db.GetTableDefs().ForEach(t =>
                        tables.Add(t.Name)
                    );
                }
                tables.ForEach(t => {
                    generator.Generate(t, outFolder);
                });

                JsonSettingWriter.SetAppSettingValue(keyName, constrArgument.Value,
                 settingsJsonOption.Value());

                Console.WriteLine("Successfully generated entity files into {0}", outFolder);
                Console.WriteLine("run `dotnet add package lazormlib` to get entities working.");

                return 0;
            });

            app.Execute(args);
        }
    }
}
