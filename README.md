# lazorm
LAZy people's OR Mapper

# What is lazorm?

There is a big framework of OR mapper in .Net ecosystem - Entity Framework.
We strongly respect it but we often feel it's just a little bit complicated.

So we made a new simple framework named lazorm.

# Key functions 

1. Entity Class generator from database 

lazorm provides a command line tool which create class files  
with full of table columns as properties.

2. Rich functions
They also have full of crud functions as default.
So all you have to do is call Get method and you will have your data brought.
You can also specify your sql to get/modify your data.

3. Source code provision

You can get all crud functions as source code generated into parcial class.
So if you think it's not your cup of tea, you can write your own methods 
into another half of parcial class, or even modify generated functions.

Isn't it a open approach to give a try?

# Installation 

Global install tool by typing:
```
dotnet tool install -g lazorm
```

# Tutorial 

You make a project by typing 
```
dotnet new console -n LazormTest
cd LazormTest
```


Then create entity from database

```
dotnet lazorm mssql "..." -out Entities
```

That's it!
You will see entity files in Entities directory.

In your program.cs file, you modify Main method

```
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

> "Mother, don't change my clothes! I can do it with my own!"

# Database first oriented

We know code first is a good approach but we chose database first.
Reason:

 It gets simpler with DB first approach - because no migration table polution nore annoying migration failure.

 