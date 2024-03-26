﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazormPageGenerator {
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System.CodeDom;
    using System;
    
    
    public partial class DetailPageWithFluxorTemplate : DetailPageWithFluxorTemplateBase {
        
        public virtual string TransformText() {
            this.GenerationEnvironment = null;
            
            #line 7 ""
            this.Write("\n@namespace ");
            
            #line default
            #line hidden
            
            #line 8 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( pageNamespace ));
            
            #line default
            #line hidden
            
            #line 8 ""
            this.Write(".Pages\n@page \"/");
            
            #line default
            #line hidden
            
            #line 9 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 9 ""
            this.Write("/edit/{");
            
            #line default
            #line hidden
            
            #line 9 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 9 ""
            this.Write("Id}\"\n@page \"/");
            
            #line default
            #line hidden
            
            #line 10 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 10 ""
            this.Write(@"/new""
@inject HttpClient httpClient
@attribute [Authorize]
@inherits Fluxor.Blazor.Web.Components.FluxorComponent;
@using System.Threading.Tasks;
@using Fluxor;
@using Lazorm;
@using Microsoft.AspNetCore.Components;
@using Lazorm.Store.States;
@using Microsoft.Extensions.Logging; 
@using Lazorm.Store.Features.");
            
            #line default
            #line hidden
            
            #line 20 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 20 ""
            this.Write("UseCase.Actions.Create");
            
            #line default
            #line hidden
            
            #line 20 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 20 ""
            this.Write(";\n@using Lazorm.Store.Features.");
            
            #line default
            #line hidden
            
            #line 21 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 21 ""
            this.Write("UseCase.Actions.Load");
            
            #line default
            #line hidden
            
            #line 21 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 21 ""
            this.Write(";\n@using Lazorm.Store.Features.");
            
            #line default
            #line hidden
            
            #line 22 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 22 ""
            this.Write("UseCase.Actions.Load");
            
            #line default
            #line hidden
            
            #line 22 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 22 ""
            this.Write(";\n@using Lazorm.Store.Features.");
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write("UseCase.Actions.Update");
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write(";\n@using Lazorm.Store.Features.");
            
            #line default
            #line hidden
            
            #line 24 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 24 ""
            this.Write("UseCase.Actions.Delete");
            
            #line default
            #line hidden
            
            #line 24 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 24 ""
            this.Write(";\n\n<h3>Edit ");
            
            #line default
            #line hidden
            
            #line 26 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 26 ""
            this.Write("</h3>\n\n@if (");
            
            #line default
            #line hidden
            
            #line 28 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 28 ""
            this.Write("State?.Value.IsLoading ?? false)\n{\n    <p><em>Loading...</em></p>\n}\nelse if (");
            
            #line default
            #line hidden
            
            #line 32 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 32 ""
            this.Write("State?.Value.HasCurrentErrors ?? false || ");
            
            #line default
            #line hidden
            
            #line 32 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 32 ""
            this.Write("State?.Value.CurrentEntity is null)\n{\n    <div class=\"d-flex flex-column \">\n     " +
                    "   <span class=\"py-2\">An error occured while loading ");
            
            #line default
            #line hidden
            
            #line 35 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 35 ""
            this.Write(" data. <br />\n\t    Try refresh page.<br />\n\t    If the problem continues, contact" +
                    " support.</span>\n    </div>\n}\nelse\n{\n    <EditForm id=\"");
            
            #line default
            #line hidden
            
            #line 42 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 42 ""
            this.Write("Form\" Model=\"@the");
            
            #line default
            #line hidden
            
            #line 42 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 42 ""
            this.Write("\" OnValidSubmit=\"@HandleValidSubmit\">\n    <DataAnnotationsValidator />\n        ");
            
            #line default
            #line hidden
            
            #line 44 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( CreateFormBody(mainContext) ));
            
            #line default
            #line hidden
            
            #line 44 ""
            this.Write("\n\t    ");
            
            #line default
            #line hidden
            
            #line 45 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( CreateChildrenTables() ));
            
            #line default
            #line hidden
            
            #line 45 ""
            this.Write("\n    </EditForm>\n\t<button class=\"btn btn-secondary\" @onclick=\"() => BackToShowPag" +
                    "e()\">Cancel</button>\n    <button type=\"submit\" form=\"");
            
            #line default
            #line hidden
            
            #line 48 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 48 ""
            this.Write("Form\" class=\"btn btn-primary\" >Update</button>\n\t<button class=\"btn btn-danger\" @o" +
                    "nclick=\"() => Delete");
            
            #line default
            #line hidden
            
            #line 49 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 49 ""
            this.Write("(");
            
            #line default
            #line hidden
            
            #line 49 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 49 ""
            this.Write("State?.Value.CurrentEntity)\">Delete</button>\n}\n\n@code {\n\t[Parameter]\n\tpublic stri" +
                    "ng ");
            
            #line default
            #line hidden
            
            #line 54 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 54 ""
            this.Write("Id {get; set;} = string.Empty;\n\n\t[Inject]\n\tprivate IState<");
            
            #line default
            #line hidden
            
            #line 57 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 57 ""
            this.Write("State>? ");
            
            #line default
            #line hidden
            
            #line 57 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 57 ""
            this.Write("State {get; set;}\n\n\t[Inject]\n\tprivate IDispatcher? dispatcher { get; set; }\n\n\t[In" +
                    "ject]\n\tNavigationManager? navigation {get; set;}\n\n\t[Inject]\n\tprivate ILogger<Edi" +
                    "t");
            
            #line default
            #line hidden
            
            #line 66 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 66 ""
            this.Write("Page>? _logger{get; set;}\n\tprivate Lazorm.");
            
            #line default
            #line hidden
            
            #line 67 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 67 ""
            this.Write("Validation the");
            
            #line default
            #line hidden
            
            #line 67 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 67 ""
            this.Write(" = new Lazorm.");
            
            #line default
            #line hidden
            
            #line 67 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 67 ""
            this.Write("Validation();\n\n\tprivate bool alertVisible = false;\n\n\tprotected override void OnIn" +
                    "itialized()\n\t{\n\t\t// Load the ");
            
            #line default
            #line hidden
            
            #line 73 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 73 ""
            this.Write(" detail on initial page navigation\n\t\tif (int.TryParse(");
            
            #line default
            #line hidden
            
            #line 74 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 74 ""
            this.Write("Id, out var parsedId))\n\t\t{\n\t\t\tvar current");
            
            #line default
            #line hidden
            
            #line 76 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 76 ""
            this.Write(" = new ");
            
            #line default
            #line hidden
            
            #line 76 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 76 ""
            this.Write("() { Id = parsedId };\n\t\t\tdispatcher?.Dispatch(new Load");
            
            #line default
            #line hidden
            
            #line 77 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 77 ""
            this.Write("Action(current");
            
            #line default
            #line hidden
            
            #line 77 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 77 ""
            this.Write("));\n\t\t}\n\t\telse\n\t\t{\n\t\t\tvar current");
            
            #line default
            #line hidden
            
            #line 81 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 81 ""
            this.Write(" = new ");
            
            #line default
            #line hidden
            
            #line 81 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 81 ""
            this.Write("();\n\t\t}\n\n\t\t// Register a state change to assign the validation fields\n\t\tif(");
            
            #line default
            #line hidden
            
            #line 85 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 85 ""
            this.Write("State != null) ");
            
            #line default
            #line hidden
            
            #line 85 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 85 ""
            this.Write("State.StateChanged += (sender, e) =>\n\t\t{\n\t\t\t_logger?.LogInformation($\"StateChange" +
                    "d start:\");\n\t\t\tif (");
            
            #line default
            #line hidden
            
            #line 88 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 88 ""
            this.Write("State.Value.CurrentEntity is null)\n\t\t\t{\n\t\t\t\treturn;\n\t\t\t}\n\t\t\t_logger?.LogInformati" +
                    "on($\"Id={");
            
            #line default
            #line hidden
            
            #line 92 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 92 ""
            this.Write("State.Value.CurrentEntity.Id} \");\n\n\t\t\tthe");
            
            #line default
            #line hidden
            
            #line 94 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 94 ""
            this.Write(" = new Lazorm.");
            
            #line default
            #line hidden
            
            #line 94 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 94 ""
            this.Write("Validation(");
            
            #line default
            #line hidden
            
            #line 94 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 94 ""
            this.Write("State.Value.CurrentEntity);\n\n\t\t\tStateHasChanged();\n\t\t\t_logger?.LogInformation($\"S" +
                    "tateChanged end:\");\n\t\t};\n\n\t\tbase.OnInitialized();\n\t}\n\n\tprotected void Delete");
            
            #line default
            #line hidden
            
            #line 103 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 103 ""
            this.Write("(");
            
            #line default
            #line hidden
            
            #line 103 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 103 ""
            this.Write(" deleting");
            
            #line default
            #line hidden
            
            #line 103 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 103 ""
            this.Write(")\n\t{\n\t\tdispatcher?.Dispatch(new Delete");
            
            #line default
            #line hidden
            
            #line 105 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 105 ""
            this.Write("Action(deleting");
            
            #line default
            #line hidden
            
            #line 105 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 105 ""
            this.Write("));\n\t\tnavigation?.NavigateTo(\"/");
            
            #line default
            #line hidden
            
            #line 106 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 106 ""
            this.Write("\");\n\t}\n\n\tprivate void BackToShowPage()\n\t{\n\t\tif (string.IsNullOrEmpty(");
            
            #line default
            #line hidden
            
            #line 111 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 111 ""
            this.Write("Id ))\n\t\t{\n\t\t\tnavigation?.NavigateTo($\"/");
            
            #line default
            #line hidden
            
            #line 113 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 113 ""
            this.Write("/\");\n\t\t}\n\t\telse\n\t\t{\n\t\t\tnavigation?.NavigateTo($\"");
            
            #line default
            #line hidden
            
            #line 117 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 117 ""
            this.Write("/show/{ ");
            
            #line default
            #line hidden
            
            #line 117 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 117 ""
            this.Write("Id }\");\n\t\t}\n\t}\n\n\tprotected void HandleValidSubmit()\n\t{\n\t\tStore");
            
            #line default
            #line hidden
            
            #line 123 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 123 ""
            this.Write("(\n\t\t\tthe");
            
            #line default
            #line hidden
            
            #line 124 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 124 ""
            this.Write(".To");
            
            #line default
            #line hidden
            
            #line 124 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 124 ""
            this.Write("()\n\t\t);\n\t\tStateHasChanged();\n\t}\n\n\tprotected void Store");
            
            #line default
            #line hidden
            
            #line 129 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 129 ""
            this.Write("(");
            
            #line default
            #line hidden
            
            #line 129 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 129 ""
            this.Write(" storing");
            
            #line default
            #line hidden
            
            #line 129 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 129 ""
            this.Write(")\n\t{\n\t\tif(0 == storing");
            
            #line default
            #line hidden
            
            #line 131 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 131 ""
            this.Write(".Id)\n\t\t{\n\t\t\tdispatcher?.Dispatch(new Create");
            
            #line default
            #line hidden
            
            #line 133 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 133 ""
            this.Write("Action(storing");
            
            #line default
            #line hidden
            
            #line 133 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 133 ""
            this.Write("));\n\t\t\tnavigation?.NavigateTo($\"/");
            
            #line default
            #line hidden
            
            #line 134 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 134 ""
            this.Write("/\");\n\t\t}\n\t\telse\n\t\t{\n\t\t\t// _logger.LogInformation($\"Dispatching: xx = {storing");
            
            #line default
            #line hidden
            
            #line 138 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 138 ""
            this.Write(".xx}\");\n\t\t\tdispatcher?.Dispatch(new Update");
            
            #line default
            #line hidden
            
            #line 139 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 139 ""
            this.Write("Action(storing");
            
            #line default
            #line hidden
            
            #line 139 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 139 ""
            this.Write("));\n\t\t\tnavigation?.NavigateTo($\"/");
            
            #line default
            #line hidden
            
            #line 140 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 140 ""
            this.Write("/show/{");
            
            #line default
            #line hidden
            
            #line 140 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 140 ""
            this.Write("State?.Value?.CurrentEntity?.Id}/\");\n\t\t}\n\n\t}\n}");
            
            #line default
            #line hidden
            return this.GenerationEnvironment.ToString();
        }
        
        public virtual void Initialize() {
        }
    }
    
    public class DetailPageWithFluxorTemplateBase {
        
        private global::System.Text.StringBuilder builder;
        
        private global::System.Collections.Generic.IDictionary<string, object> session;
        
        private global::System.CodeDom.Compiler.CompilerErrorCollection errors;
        
        private string currentIndent = string.Empty;
        
        private global::System.Collections.Generic.Stack<int> indents;
        
        private ToStringInstanceHelper _toStringHelper = new ToStringInstanceHelper();
        
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session {
            get {
                return this.session;
            }
            set {
                this.session = value;
            }
        }
        
        public global::System.Text.StringBuilder GenerationEnvironment {
            get {
                if ((this.builder == null)) {
                    this.builder = new global::System.Text.StringBuilder();
                }
                return this.builder;
            }
            set {
                this.builder = value;
            }
        }
        
        protected global::System.CodeDom.Compiler.CompilerErrorCollection Errors {
            get {
                if ((this.errors == null)) {
                    this.errors = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errors;
            }
        }
        
        public string CurrentIndent {
            get {
                return this.currentIndent;
            }
        }
        
        private global::System.Collections.Generic.Stack<int> Indents {
            get {
                if ((this.indents == null)) {
                    this.indents = new global::System.Collections.Generic.Stack<int>();
                }
                return this.indents;
            }
        }
        
        public ToStringInstanceHelper ToStringHelper {
            get {
                return this._toStringHelper;
            }
        }
        
        public void Error(string message) {
            this.Errors.Add(new global::System.CodeDom.Compiler.CompilerError(null, -1, -1, null, message));
        }
        
        public void Warning(string message) {
            global::System.CodeDom.Compiler.CompilerError val = new global::System.CodeDom.Compiler.CompilerError(null, -1, -1, null, message);
            val.IsWarning = true;
            this.Errors.Add(val);
        }
        
        public string PopIndent() {
            if ((this.Indents.Count == 0)) {
                return string.Empty;
            }
            int lastPos = (this.currentIndent.Length - this.Indents.Pop());
            string last = this.currentIndent.Substring(lastPos);
            this.currentIndent = this.currentIndent.Substring(0, lastPos);
            return last;
        }
        
        public void PushIndent(string indent) {
            this.Indents.Push(indent.Length);
            this.currentIndent = (this.currentIndent + indent);
        }
        
        public void ClearIndent() {
            this.currentIndent = string.Empty;
            this.Indents.Clear();
        }
        
        public void Write(string textToAppend) {
            this.GenerationEnvironment.Append(textToAppend);
        }
        
        public void Write(string format, params object[] args) {
            this.GenerationEnvironment.AppendFormat(format, args);
        }
        
        public void WriteLine(string textToAppend) {
            this.GenerationEnvironment.Append(this.currentIndent);
            this.GenerationEnvironment.AppendLine(textToAppend);
        }
        
        public void WriteLine(string format, params object[] args) {
            this.GenerationEnvironment.Append(this.currentIndent);
            this.GenerationEnvironment.AppendFormat(format, args);
            this.GenerationEnvironment.AppendLine();
        }
        
        public class ToStringInstanceHelper {
            
            private global::System.IFormatProvider formatProvider = global::System.Globalization.CultureInfo.InvariantCulture;
            
            public global::System.IFormatProvider FormatProvider {
                get {
                    return this.formatProvider;
                }
                set {
                    if ((value != null)) {
                        this.formatProvider = value;
                    }
                }
            }
            
            public string ToStringWithCulture(object objectToConvert) {
                if ((objectToConvert == null)) {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                global::System.Type type = objectToConvert.GetType();
                global::System.Type iConvertibleType = typeof(global::System.IConvertible);
                if (iConvertibleType.IsAssignableFrom(type)) {
                    return ((global::System.IConvertible)(objectToConvert)).ToString(this.formatProvider);
                }
                global::System.Reflection.MethodInfo methInfo = type.GetMethod("ToString", new global::System.Type[] {
                            iConvertibleType});
                if ((methInfo != null)) {
                    return ((string)(methInfo.Invoke(objectToConvert, new object[] {
                                this.formatProvider})));
                }
                return objectToConvert.ToString();
            }
        }
    }
}
