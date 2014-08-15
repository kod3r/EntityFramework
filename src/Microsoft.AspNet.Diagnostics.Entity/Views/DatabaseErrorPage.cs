namespace Microsoft.AspNet.Diagnostics.Entity.Views
{
#line 1 "DatabaseErrorPage.cshtml"
using System

#line default
#line hidden
    ;
#line 2 "DatabaseErrorPage.cshtml"
using System.Linq

#line default
#line hidden
    ;
#line 3 "DatabaseErrorPage.cshtml"
using System.Globalization

#line default
#line hidden
    ;
    using System.Threading.Tasks;

    public class DatabaseErrorPage : Microsoft.AspNet.Diagnostics.Views.BaseView
    {
#line 11 "DatabaseErrorPage.cshtml"

    public Microsoft.AspNet.Diagnostics.Entity.Views.DatabaseErrorPageModel Model { get; set; }

#line default
#line hidden
        #line hidden
        public DatabaseErrorPage()
        {
        }

        #pragma warning disable 1998
        public override async Task ExecuteAsync()
        {
#line 4 "DatabaseErrorPage.cshtml"
  
    Response.StatusCode = 500;
    // TODO: Response.ReasonPhrase = "Internal Server Error";
    Response.ContentType = "text/html";
    Response.ContentLength = null; // Clear any prior Content-Length

#line default
#line hidden

            WriteLiteral("\r\n");
            WriteLiteral("<!DOCTYPE html>\r\n\r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r" +
"\n    <meta charset=\"utf-8\" />\r\n    <title>Internal Server Error</title>\r\n    <st" +
"yle>\r\n        body {\r\n    font-family: 'Segoe UI', Tahoma, Arial, Helvetica, sans-serif;\r\n    font-size: .813em;\r\n    line-height: 1.4em;\r\n    color: #222;\r\n}\r\n\r\nh1, h2, h3, h4, h5 {\r\n    font-weight: 100;\r\n}\r\n\r\nh1 {\r\n    color: #44525e;\r\n    margin: 15px 0 15px 0;\r\n}\r\n\r\nh2 {\r\n    margin: 10px 5px 0 0;\r\n}\r\n\r\nh3 {\r\n    color: #363636;\r\n    margin: 5px 5px 0 0;\r\n}\r\n\r\ncode {\r\n    font-family: Consolas, \"Courier New\", courier, monospace;\r\n}\r\n\r\na {\r\n    color: #1ba1e2;\r\n    text-decoration: none;\r\n}\r\n\r\n    a:hover {\r\n        color: #13709e;\r\n        text-decoration: underline;\r\n    }\r\n\r\nhr {\r\n    border: 1px #ddd solid;\r\n}\r\n\r\nbody .titleerror {\r\n    padding: 3px;\r\n}\r\n\r\n#applyMigrations {\r\n    font-size: 14px;\r\n    background: #44c5f2;\r\n    color: #ffffff;\r\n    display: inline-block;\r\n    padding: 6px 12px;\r\n    margin-bottom: 0;\r\n    font-weight: normal;\r\n    text-align: center;\r\n    white-space: nowrap;\r\n    vertical-align: middle;\r\n    cursor: pointer;\r\n    border: 1px solid transparent;\r\n}\r\n\r\n    #applyMigrations:disabled {\r\n        background-color: #a9e4f9;\r\n        border-color: #44c5f2;\r\n    }\r\n\r\n.error {\r\n    color: red;\r\n}\r\n\r\n.expanded {\r\n    display: block;\r\n}\r\n\r\n.collapsed {\r\n    display: none;\r\n}\r\n\r\n        ");
            Write(
#line 22 "DatabaseErrorPage.cshtml"
         string.Empty

#line default
#line hidden
            );

            WriteLiteral("\r\n    </style>\r\n</head>\r\n<body>\r\n    <h1>A database operartion failed while proce" +
"ssing the request.</h1>\r\n");
#line 27 "DatabaseErrorPage.cshtml"
    

#line default
#line hidden

#line 27 "DatabaseErrorPage.cshtml"
     if (Model.Options.ShowExceptionDetails)
    {

#line default
#line hidden

            WriteLiteral("        <p>\r\n");
#line 30 "DatabaseErrorPage.cshtml"
            

#line default
#line hidden

#line 30 "DatabaseErrorPage.cshtml"
             for (Exception ex = Model.Exception; ex != null; ex = ex.InnerException)
            {

#line default
#line hidden

            WriteLiteral("                <span>");
            Write(
#line 32 "DatabaseErrorPage.cshtml"
                       ex.GetType().Name

#line default
#line hidden
            );

            WriteLiteral(": ");
            Write(
#line 32 "DatabaseErrorPage.cshtml"
                                           ex.Message

#line default
#line hidden
            );

            WriteLiteral("</span>\r\n                <br />\r\n");
#line 34 "DatabaseErrorPage.cshtml"
            }

#line default
#line hidden

            WriteLiteral("        </p>\r\n        <hr />\r\n");
#line 37 "DatabaseErrorPage.cshtml"
    }

#line default
#line hidden

            WriteLiteral("\r\n");
#line 39 "DatabaseErrorPage.cshtml"
    

#line default
#line hidden

#line 39 "DatabaseErrorPage.cshtml"
     if (Model.Options.ShowMigrationStatus)
    {
        

#line default
#line hidden

#line 41 "DatabaseErrorPage.cshtml"
         if (!Model.DatabaseExists && !Model.PendingMigrations.Any())
        {

#line default
#line hidden

            WriteLiteral("            <h2>");
            Write(
#line 43 "DatabaseErrorPage.cshtml"
                 Microsoft.AspNet.Diagnostics.Entity.Strings.FormatDatabaseErrorPage_NoDbOrMigrationsTitle(Model.ContextType.Name)

#line default
#line hidden
            );

            WriteLiteral("</h2>\r\n            <p>");
            Write(
#line 44 "DatabaseErrorPage.cshtml"
                Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_NoDbOrMigrationsInfo

#line default
#line hidden
            );

            WriteLiteral("</p>\r\n            <code> ");
            Write(
#line 45 "DatabaseErrorPage.cshtml"
                    Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_AddMigrationCommand

#line default
#line hidden
            );

            WriteLiteral(" </code>\r\n            <br />\r\n            <code> ");
            Write(
#line 47 "DatabaseErrorPage.cshtml"
                    Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_UpdateDatabaseCommand

#line default
#line hidden
            );

            WriteLiteral(" </code>\r\n            <hr />\r\n");
#line 49 "DatabaseErrorPage.cshtml"
        }
        else
        {
            if (Model.PendingMigrations.Any())
            {

#line default
#line hidden

            WriteLiteral("                <h2>");
            Write(
#line 54 "DatabaseErrorPage.cshtml"
                     Microsoft.AspNet.Diagnostics.Entity.Strings.FormatDatabaseErrorPage_Title(Model.ContextType.Name)

#line default
#line hidden
            );

            WriteLiteral("</h2>\r\n                <p>");
            Write(
#line 55 "DatabaseErrorPage.cshtml"
                    Microsoft.AspNet.Diagnostics.Entity.Strings.FormatDatabaseErrorPage_PendingMigrationsInfo(Model.ContextType.Name)

#line default
#line hidden
            );

            WriteLiteral("</p>\r\n                <ul>\r\n");
#line 57 "DatabaseErrorPage.cshtml"
                    

#line default
#line hidden

#line 57 "DatabaseErrorPage.cshtml"
                     foreach (var migration in Model.PendingMigrations)
                    {

#line default
#line hidden

            WriteLiteral("                        <li>");
            Write(
#line 59 "DatabaseErrorPage.cshtml"
                             migration

#line default
#line hidden
            );

            WriteLiteral("</li>\r\n");
#line 60 "DatabaseErrorPage.cshtml"
                    }

#line default
#line hidden

            WriteLiteral("\r\n                </ul>\r\n");
#line 62 "DatabaseErrorPage.cshtml"
                

#line default
#line hidden

#line 62 "DatabaseErrorPage.cshtml"
                 if (Model.Options.EnableMigrationCommands)
                {

#line default
#line hidden

            WriteLiteral("                    <p>\r\n                        <button id=\"applyMigrations\" onc" +
"lick=\"ApplyMigrations()\">");
            Write(
#line 65 "DatabaseErrorPage.cshtml"
                                                                                  Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_ApplyMigrationsButton

#line default
#line hidden
            );

            WriteLiteral(@"</button>
                        <span id=""applyMigrationsError"" class=""error""></span>
                        <span id=""applyMigrationsSuccess""></span>
                    </p>
                    <script>
                        function ApplyMigrations() {
                            applyMigrations.disabled = true;
                            applyMigrationsError.innerHTML = """";
                            applyMigrations.innerHTML = """);
            Write(
#line 73 "DatabaseErrorPage.cshtml"
                                                          Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_ApplyMigrationsButtonRunning

#line default
#line hidden
            );

            WriteLiteral("\";\r\n\r\n                            var req = new XMLHttpRequest();\r\n              " +
"              req.open(\"POST\", \"");
            Write(
#line 76 "DatabaseErrorPage.cshtml"
                                               Model.Options.MigrationsEndPointPath.Value

#line default
#line hidden
            );

            WriteLiteral("\", true);\r\n                            var params = \"context=\" + encodeURICompone" +
"nt(\"");
            Write(
#line 77 "DatabaseErrorPage.cshtml"
                                                                           Model.ContextType.AssemblyQualifiedName

#line default
#line hidden
            );

            WriteLiteral(@""");
                            req.setRequestHeader(""Content-type"", ""application/x-www-form-urlencoded"");
                            req.setRequestHeader(""Content-length"", params.length);
                            req.setRequestHeader(""Connection"", ""close"");

                            req.onload = function (e) {
                                if (req.status == 204) {
                                    applyMigrations.innerHTML = """);
            Write(
#line 84 "DatabaseErrorPage.cshtml"
                                                                  Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_ApplyMigrationsButtonDone

#line default
#line hidden
            );

            WriteLiteral("\";\r\n                                    applyMigrationsSuccess.innerHTML = \"<a hr" +
"ef=\'.\'>");
            Write(
#line 85 "DatabaseErrorPage.cshtml"
                                                                                     Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_MigrationsAppliedRefresh

#line default
#line hidden
            );

            WriteLiteral(@"</a>"";
                                }
                                else {
                                    ErrorApplyingMigrations();
                                }
                            };

                            req.onerror = function (e) {
                                ErrorApplyingMigrations();
                            };

                            req.send(params);
                        }

                        function ErrorApplyingMigrations() {
                            applyMigrations.innerHTML = """);
            Write(
#line 100 "DatabaseErrorPage.cshtml"
                                                          Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_ApplyMigrationsButton

#line default
#line hidden
            );

            WriteLiteral("\";\r\n                            applyMigrationsError.innerHTML = \"");
            Write(
#line 101 "DatabaseErrorPage.cshtml"
                                                               Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_ApplyMigrationsFailed

#line default
#line hidden
            );

            WriteLiteral("\";\r\n                            applyMigrations.disabled = false;\r\n              " +
"          }\r\n                    </script>\r\n");
#line 105 "DatabaseErrorPage.cshtml"
                }

#line default
#line hidden

#line 105 "DatabaseErrorPage.cshtml"
                 

#line default
#line hidden

            WriteLiteral("                <p>");
            Write(
#line 106 "DatabaseErrorPage.cshtml"
                    Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_HowToApplyFromCmd

#line default
#line hidden
            );

            WriteLiteral("</p>\r\n                <code>");
            Write(
#line 107 "DatabaseErrorPage.cshtml"
                       Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_UpdateDatabaseCommand

#line default
#line hidden
            );

            WriteLiteral("</code>\r\n                <hr />\r\n");
#line 109 "DatabaseErrorPage.cshtml"
            }
            else if (Model.PendingModelChanges)
            {

#line default
#line hidden

            WriteLiteral("                <h2>");
            Write(
#line 112 "DatabaseErrorPage.cshtml"
                     Microsoft.AspNet.Diagnostics.Entity.Strings.FormatDatabaseErrorPage_PendingChangesTitle(Model.ContextType.Name)

#line default
#line hidden
            );

            WriteLiteral("</h2>\r\n                <p>");
            Write(
#line 113 "DatabaseErrorPage.cshtml"
                    Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_PendingChangesInfo

#line default
#line hidden
            );

            WriteLiteral("</p>\r\n                <code>");
            Write(
#line 114 "DatabaseErrorPage.cshtml"
                       Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_AddMigrationCommand

#line default
#line hidden
            );

            WriteLiteral("</code>\r\n                <br />\r\n                <code>");
            Write(
#line 116 "DatabaseErrorPage.cshtml"
                       Microsoft.AspNet.Diagnostics.Entity.Strings.DatabaseErrorPage_UpdateDatabaseCommand

#line default
#line hidden
            );

            WriteLiteral("</code>\r\n                <hr />\r\n");
#line 118 "DatabaseErrorPage.cshtml"
            }
        }

#line default
#line hidden

#line 119 "DatabaseErrorPage.cshtml"
         
    }

#line default
#line hidden

            WriteLiteral("</body>\r\n</html>\r\n");
        }
        #pragma warning restore 1998
    }
}
