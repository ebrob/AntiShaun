AntiShaun
=========
AntiShaun is a POCO-model-based reporting library. It allows ODT (Text) and ODS (Spreadsheet) 
documents to be used as report templates by inserting "tags" in appropriate places. These tags are in 
standard ASP.NET Razor format. The library compiles this document into a typesafe compiled Razor template. 
The programmer can then apply the model to the compiled template and generate the output document.

We deliberately do not attempt to solve all aspects of the report generation process. For example,
we generate native ODT and ODS files and assume that the application will have the means to convert these
to PDF or Microsoft formats as necessary.

One of the goals of the project is to create a plugin for OpenOffice/LibreOffice which would
simplify the template document creation.

A final goal of the project was to serve as the "pet project" for a promising apprentice, Calvin Scott, 
who is [blogging](http://apprenticeship.onelittleoutlet.com/) his experiences.

Project status
--------------
AntiShaun is in early alpha state and is not recommended for serious use.

Technical goals
---------------
* Native .NET.
* Use standard tools such as [Razor](http://en.wikipedia.org/wiki/Microsoft_ASP.NET_Razor_view_engine).
* Use standard [POCO](http://en.wikipedia.org/wiki/Plain_Old_CLR_Object) models for the data source.
* Use a standard office file format for authoring - we selected [ODF](http://en.wikipedia.org/wiki/OpenDocument).
* Each template is in a native format -- word processing or spreadsheet.
* Spreadsheet should support actual cell types and formulas
* Compile-time model type checking.
* OpenOffice/LibreOffice plugin to assist in design and validation of reports

Similar projects
----------------
There have been a few other similar projects, but none met our goals.
* [NTemplates](http://ntemplates.codeplex.com/) - .NET, RTF only
* [RazorPDF](http://nyveldt.com/blog/page/razorpdf) - .NET, PDF only
* [JODReports](http://jodreports.sourceforge.net/) - Java
* [XDocRepor](https://code.google.com/p/xdocreport/) - Java
