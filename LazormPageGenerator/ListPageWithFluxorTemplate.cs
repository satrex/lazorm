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
    
    
    public partial class ListPageWithFluxorTemplate : ListPageWithFluxorTemplateBase {
        
        public virtual string TransformText() {
            this.GenerationEnvironment = null;
            
            #line 7 ""
            this.Write("@namespace ");
            
            #line default
            #line hidden
            
            #line 7 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( pageNamespace ));
            
            #line default
            #line hidden
            
            #line 7 ""
            this.Write(".Pages\n@page \"/");
            
            #line default
            #line hidden
            
            #line 8 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 8 ""
            this.Write(@"""
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
            
            #line 18 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 18 ""
            this.Write("UseCase.Actions.Create");
            
            #line default
            #line hidden
            
            #line 18 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 18 ""
            this.Write(";\n@using Lazorm.Store.Features.");
            
            #line default
            #line hidden
            
            #line 19 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 19 ""
            this.Write("UseCase.Actions.Load");
            
            #line default
            #line hidden
            
            #line 19 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 19 ""
            this.Write(";\n@using Lazorm.Store.Features.");
            
            #line default
            #line hidden
            
            #line 20 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 20 ""
            this.Write("UseCase.Actions.Load");
            
            #line default
            #line hidden
            
            #line 20 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
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
            this.Write("UseCase.Actions.Update");
            
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
            this.Write("UseCase.Actions.Delete");
            
            #line default
            #line hidden
            
            #line 22 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 22 ""
            this.Write(";\n<h1>");
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write("</h1> \n\n@if (");
            
            #line default
            #line hidden
            
            #line 25 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 25 ""
            this.Write("State?.Value.IsLoading ?? false)\n{\n    <p><em>Loading...</em></p>\n}\nelse \n{\n<");
            
            #line default
            #line hidden
            
            #line 31 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 31 ""
            this.Write("TableComponent ");
            
            #line default
            #line hidden
            
            #line 31 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 31 ""
            this.Write("=");
            
            #line default
            #line hidden
            
            #line 31 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 31 ""
            this.Write("State?.Value?.Current");
            
            #line default
            #line hidden
            
            #line 31 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 31 ""
            this.Write(" OnRowClickCallback=\"@ShowDetail\" >\n</");
            
            #line default
            #line hidden
            
            #line 32 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 32 ""
            this.Write("TableComponent>\n<button class=\"btn btn-success\" @onclick=\"Add");
            
            #line default
            #line hidden
            
            #line 33 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 33 ""
            this.Write("\">Add ");
            
            #line default
            #line hidden
            
            #line 33 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 33 ""
            this.Write("</button>\n}\n\n@code {\n    [Inject]\n\tprivate IState<");
            
            #line default
            #line hidden
            
            #line 38 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 38 ""
            this.Write("State>? ");
            
            #line default
            #line hidden
            
            #line 38 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 38 ""
            this.Write("State {get; set;}\n\n\t[Inject]\n\tprivate IDispatcher? dispatcher { get; set; }\n\n\t[In" +
                    "ject]\n\tNavigationManager? navigation {get; set;}\n\n\t[Inject]\n\tprivate ILogger<");
            
            #line default
            #line hidden
            
            #line 47 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 47 ""
            this.Write("Page>? _logger{get; set;}\n\n\tprivate bool alertVisible = false;\n\n\tprotected overri" +
                    "de void OnInitialized()\n\t{\n\t\t// Load ");
            
            #line default
            #line hidden
            
            #line 53 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 53 ""
            this.Write(" on initial page navigation\n\t\tdispatcher?.Dispatch(new Load");
            
            #line default
            #line hidden
            
            #line 54 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 54 ""
            this.Write("Action());\n\n\t\tbase.OnInitialized();\n\t}\n\n\tprivate void ShowDetail(");
            
            #line default
            #line hidden
            
            #line 59 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 59 ""
            this.Write(" ");
            
            #line default
            #line hidden
            
            #line 59 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 59 ""
            this.Write(")\n\t{\n\t\tnavigation?.NavigateTo($\"");
            
            #line default
            #line hidden
            
            #line 61 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 61 ""
            this.Write("/show/{ ");
            
            #line default
            #line hidden
            
            #line 61 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 61 ""
            this.Write(".Id }\");\n\t}\n\n    private void Add");
            
            #line default
            #line hidden
            
            #line 64 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 64 ""
            this.Write("()\n    {\n        // TODO: Implement adding ");
            
            #line default
            #line hidden
            
            #line 66 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 66 ""
            this.Write(" functionality\n\t\tnavigation?.NavigateTo($\"");
            
            #line default
            #line hidden
            
            #line 67 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 67 ""
            this.Write("/new\");\n    }\n\n    private void Edit");
            
            #line default
            #line hidden
            
            #line 70 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 70 ""
            this.Write("(");
            
            #line default
            #line hidden
            
            #line 70 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 70 ""
            this.Write(" ");
            
            #line default
            #line hidden
            
            #line 70 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 70 ""
            this.Write(")\n    {\n        // TODO: Implement editing ");
            
            #line default
            #line hidden
            
            #line 72 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 72 ""
            this.Write(" functionality\n\t\tnavigation?.NavigateTo($\"");
            
            #line default
            #line hidden
            
            #line 73 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 73 ""
            this.Write("/edit/{ ");
            
            #line default
            #line hidden
            
            #line 73 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 73 ""
            this.Write(".Id }\");\n    }\n\n}");
            
            #line default
            #line hidden
            return this.GenerationEnvironment.ToString();
        }
        
        public virtual void Initialize() {
        }
    }
    
    public class ListPageWithFluxorTemplateBase {
        
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
