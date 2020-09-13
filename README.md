# lazorm
LAZy people's OR Mapper

## What is lazorm?

Lazorm is a super simple OR mapper library for .Net Core.

## one minute demo(youtube link)

[![Watch the video](https://img.youtube.com/vi/qZ1XBQqL2YE/hqdefault.jpg)](https://www.youtube.com/watch?v=qZ1XBQqL2YE)

## Key functions 

1. Entity Class generator from database 

lazorm provides a command line tool which create class files  
with full of table columns as properties.

2. Rich functions on coding

They also have full of crud functions as default.
So all you have to do is call Get method and you will have your data brought.
You can also specify your sql to get/modify your data.

3. Source code provision

You can get all crud functions as source code generated into parcial class.
So if you think it's not your cup of tea, you can write your own methods 
into another half of parcial class, or even modify generated functions.

Isn't it a open approach to give a try?

## Installation 

### ** Important ** 

lazorm has two part of library: 

1. code generator (as a .Net Core tool - also a.k.a. tool-path tool)
2. library (as a nuget package)

Global install tool by typing:

``` shell:Terminal
dotnet tool install -g lazorm
```

Or local install tool on your project if you like:

``` shell:Terminal @path/of/ProjectFolder
dotnet new tool-manifest
dotnet tool install lazorm
```

Then you go 2 things respectively on your project:

``` shell:Terminal @path/of/ProjectFolder
echo "creating entity files into Entities/AutoGenerated dir"
dotnet lazorm mssql "your connectionstring here"

echo "installing library that entities depends"
dotnet add package lazorm
```

# Tutorial 

You make a project by typing 

``` shell:Terminal
dotnet new console -n LazormTest
cd LazormTest
```


Then create entity from database

``` shell:Terminal @path/of/ProjectFolder
echo "creating entity files into Entities/AutoGenerated dir"
dotnet lazorm mssql "your connectionstring here"

echo "installing library that entities depends"
dotnet add package lazorm
```

That's it!
You will see entity files in Entities directory.

Don't forget to install package lazormlib. Because entity files do need it to work with.

In your program.cs file, you modify Main method

``` csharp:Program.cs
var countries = Lazorm.Country.Get();
foreach(var country in countries)
{
    Console.WriteLine("Name: {0}", country.Name);
}
```

You run project with F5,
That's it!
You will see the name of countries there.

Object.Get and Object.SaveChanges are essentials of this framework.
NO MANAGERS! Because our entities can do everything by their own.

> "Mom, don't change my clothes! I can do it with my own!"

# Database first oriented

We know code first is a good approach but we chose database first.
Reason:

 It gets simpler with DB first approach - because no migration table polution nore annoying migration failure.

 
 ---

### Release Notes

<dl>
<dt>0.2.3</dt><dd>Bug fixed.</dd>
<dt>0.2.2</dt><dd>Changed tables arg into option (if not specified, then create all tables out of schema)</dd>
<dt>0.1.18</dt><dd>Added function to create new folder when -o option is specified</dd>
<dt>0.1.16</dt><dd>Added functionality to write connection string into appsettings.json</dd>
</dl>
