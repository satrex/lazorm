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
    using System;
    
    
    public partial class DetailPageWithFluxorCsTemplate : DetailPageWithFluxorCsTemplateBase {
        
        public virtual string TransformText() {
            this.GenerationEnvironment = null;
            
            #line 6 ""
            this.Write("\n\nusing System.Threading.Tasks;\nusing Fluxor;\nusing Lazorm;\nusing Microsoft.AspNe" +
                    "tCore.Components;\nusing Lazorm.Store.States;\nusing Microsoft.Extensions.Logging;" +
                    " \nusing Lazorm.Store.Features.");
            
            #line default
            #line hidden
            
            #line 14 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 14 ""
            this.Write("UseCase.Actions.Create");
            
            #line default
            #line hidden
            
            #line 14 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 14 ""
            this.Write(";\nusing Lazorm.Store.Features.");
            
            #line default
            #line hidden
            
            #line 15 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 15 ""
            this.Write("UseCase.Actions.Load");
            
            #line default
            #line hidden
            
            #line 15 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 15 ""
            this.Write(";\nusing Lazorm.Store.Features.");
            
            #line default
            #line hidden
            
            #line 16 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 16 ""
            this.Write("UseCase.Actions.Load");
            
            #line default
            #line hidden
            
            #line 16 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 16 ""
            this.Write(";\nusing Lazorm.Store.Features.");
            
            #line default
            #line hidden
            
            #line 17 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 17 ""
            this.Write("UseCase.Actions.Update");
            
            #line default
            #line hidden
            
            #line 17 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 17 ""
            this.Write(";\nusing Lazorm.Store.Features.");
            
            #line default
            #line hidden
            
            #line 18 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 18 ""
            this.Write("UseCase.Actions.Delete");
            
            #line default
            #line hidden
            
            #line 18 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 18 ""
            this.Write(";\n\npublic partial class ");
            
            #line default
            #line hidden
            
            #line 20 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 20 ""
            this.Write("DetailPage: Fluxor.Blazor.Web.Components.FluxorComponent \n{\n    [Parameter]\n    p" +
                    "ublic string ");
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write("Id {get; set;} = string.Empty;\n\n    [Inject]\n    private IState<");
            
            #line default
            #line hidden
            
            #line 26 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 26 ""
            this.Write("State>? ");
            
            #line default
            #line hidden
            
            #line 26 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 26 ""
            this.Write("State {get; set;}\n\n    [Inject]\n    private IDispatcher? dispatcher { get; set; }" +
                    "\n\n    [Inject]\n    NavigationManager? Navigation {get; set;}\n\n    [Inject]\n    p" +
                    "rivate ILogger<");
            
            #line default
            #line hidden
            
            #line 35 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 35 ""
            this.Write("DetailPage>? _logger{get; set;}\n    private Lazorm.Validation.");
            
            #line default
            #line hidden
            
            #line 36 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 36 ""
            this.Write("Validation the");
            
            #line default
            #line hidden
            
            #line 36 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 36 ""
            this.Write(" = new Lazorm.Validation.");
            
            #line default
            #line hidden
            
            #line 36 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 36 ""
            this.Write("Validation();\n\n    private bool alertVisible = false;\n\n    protected override voi" +
                    "d OnInitialized()\n    {\n        // Load the ");
            
            #line default
            #line hidden
            
            #line 42 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 42 ""
            this.Write(" detail on initial page navigation\n        if (int.TryParse(");
            
            #line default
            #line hidden
            
            #line 43 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 43 ""
            this.Write("Id, out var parsedId))\n        {\n            var current");
            
            #line default
            #line hidden
            
            #line 45 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 45 ""
            this.Write(" = new ");
            
            #line default
            #line hidden
            
            #line 45 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 45 ""
            this.Write("() { Id = parsedId };\n            dispatcher?.Dispatch(new Load");
            
            #line default
            #line hidden
            
            #line 46 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 46 ""
            this.Write("Action(current");
            
            #line default
            #line hidden
            
            #line 46 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 46 ""
            this.Write("));\n        }\n        else\n        {\n            var current");
            
            #line default
            #line hidden
            
            #line 50 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 50 ""
            this.Write(" = new ");
            
            #line default
            #line hidden
            
            #line 50 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 50 ""
            this.Write("();\n        }\n\n        // Register a state change to assign the validation fields" +
                    "\n        if(");
            
            #line default
            #line hidden
            
            #line 54 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 54 ""
            this.Write("State != null) ");
            
            #line default
            #line hidden
            
            #line 54 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 54 ""
            this.Write(@"State.StateChanged += (sender, state) =>
        {
            _logger.LogInformation($""StateChanged start:"");
            if (state.CurrentEntity is null)
            {
                return;
            }
            _logger.LogInformation($""Name={state.CurrentEntity.Name} Phone={state.CurrentEntity.Phone}"");

            the");
            
            #line default
            #line hidden
            
            #line 63 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 63 ""
            this.Write(" = new Lazorm.Validation.");
            
            #line default
            #line hidden
            
            #line 63 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 63 ""
            this.Write("Validation(state.CurrentEntity);\n\n            StateHasChanged();\n            _log" +
                    "ger.LogInformation($\"StateChanged end:\");\n        };\n\n        base.OnInitialized" +
                    "();\n    }\n\n    protected void Delete");
            
            #line default
            #line hidden
            
            #line 72 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 72 ""
            this.Write("(");
            
            #line default
            #line hidden
            
            #line 72 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 72 ""
            this.Write(" deleting");
            
            #line default
            #line hidden
            
            #line 72 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 72 ""
            this.Write(")\n    {\n        dispatcher?.Dispatch(new Delete");
            
            #line default
            #line hidden
            
            #line 74 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 74 ""
            this.Write("Action(deleting");
            
            #line default
            #line hidden
            
            #line 74 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 74 ""
            this.Write("));\n        Navigation?.NavigateTo(\"/");
            
            #line default
            #line hidden
            
            #line 75 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 75 ""
            this.Write("\");\n    }\n    private void StartEdition(");
            
            #line default
            #line hidden
            
            #line 77 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 77 ""
            this.Write(" ");
            
            #line default
            #line hidden
            
            #line 77 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 77 ""
            this.Write(")\n    {\n        Navigation?.NavigateTo($\"");
            
            #line default
            #line hidden
            
            #line 79 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 79 ""
            this.Write("/{ ");
            
            #line default
            #line hidden
            
            #line 79 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNameSingular ));
            
            #line default
            #line hidden
            
            #line 79 ""
            this.Write(".Id }\");\n    }\n    protected void HandleValidSubmit()\n    {\n        Store");
            
            #line default
            #line hidden
            
            #line 83 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 83 ""
            this.Write("(\n            the");
            
            #line default
            #line hidden
            
            #line 84 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNameSingular ));
            
            #line default
            #line hidden
            
            #line 84 ""
            this.Write(@".ToPatient()
        );
        StateHasChanged();
    }

    protected void StorePatient(Patient storingPatient)
    {
        if(0 == storingPatient.Id)
        {
            dispatcher?.Dispatch(new CreatePatientAction(storingPatient));
        }
        else
        {
            _logger.LogInformation($""Dispatching: phone= {storingPatient.Phone}"");
            dispatcher?.Dispatch(new UpdatePatientAction(storingPatient));
        }

        Navigation?.NavigateTo($""/");
            
            #line default
            #line hidden
            
            #line 101 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityNamePlural ));
            
            #line default
            #line hidden
            
            #line 101 ""
            this.Write("/{");
            
            #line default
            #line hidden
            
            #line 101 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 101 ""
            this.Write("State?.Value?.CurrentEntity?.Id}/consultations\");\n    }\n}");
            
            #line default
            #line hidden
            return this.GenerationEnvironment.ToString();
        }
        
        public virtual void Initialize() {
        }
    }
    
    public class DetailPageWithFluxorCsTemplateBase {
        
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