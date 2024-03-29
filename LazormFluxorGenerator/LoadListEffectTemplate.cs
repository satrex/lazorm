﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazormFluxorGenerator {
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    
    public partial class LoadListEffectTemplate : LoadListEffectTemplateBase {
        
        public virtual string TransformText() {
            this.GenerationEnvironment = null;
            
            #line 6 ""
            this.Write("\n");
            
            #line default
            #line hidden
            
            #line 7 ""
  
string[] cruds = new string[] {"Create"}; 
string[] actionTypes = new string[] {"", "Success"};

string EntityClass = entityClassName;
string memberEntity = entityClassNamePlural;
string localEntity = entityClassNamePlural.ToLower();

   string actionClassName = crud + entityClassNamePlural + actionType + "Action"; 
   string effectClassName = crud + entityClassNamePlural + actionType + "Effect"; 

            
            #line default
            #line hidden
            
            #line 18 ""
            this.Write("\nusing System;\nusing System.Threading.Tasks;\nusing Fluxor;\nusing Lazorm;\nusing ");
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( namespaceText ));
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write(".Store.Features.");
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassName ));
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write("UseCase.Actions.");
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( crud ));
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 23 ""
            this.Write(";\n\nnamespace ");
            
            #line default
            #line hidden
            
            #line 25 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( namespaceText ));
            
            #line default
            #line hidden
            
            #line 25 ""
            this.Write(".Store.Features.");
            
            #line default
            #line hidden
            
            #line 25 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 25 ""
            this.Write(".Effects\n{ \n    public partial class ");
            
            #line default
            #line hidden
            
            #line 27 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( effectClassName ));
            
            #line default
            #line hidden
            
            #line 27 ""
            this.Write(": Effect<");
            
            #line default
            #line hidden
            
            #line 27 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( actionClassName ));
            
            #line default
            #line hidden
            
            #line 27 ""
            this.Write(">\n    {\n        private readonly ILogger<");
            
            #line default
            #line hidden
            
            #line 29 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( effectClassName ));
            
            #line default
            #line hidden
            
            #line 29 ""
            this.Write("> _logger;\n\n        public ");
            
            #line default
            #line hidden
            
            #line 31 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( effectClassName ));
            
            #line default
            #line hidden
            
            #line 31 ""
            this.Write("(ILogger<");
            
            #line default
            #line hidden
            
            #line 31 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( effectClassName ));
            
            #line default
            #line hidden
            
            #line 31 ""
            this.Write("> logger) =>\n            _logger = logger;\n\n");
            
            #line default
            #line hidden
            
            #line 34 ""
 if (actionType == "") { 
            
            #line default
            #line hidden
            
            #line 35 ""
            this.Write("        public override async Task HandleAsync(");
            
            #line default
            #line hidden
            
            #line 35 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( actionClassName ));
            
            #line default
            #line hidden
            
            #line 35 ""
            this.Write(" action, IDispatcher dispatcher)\n        {\n            try\n            {\n        " +
                    "        _logger.LogInformation($\"Loading ");
            
            #line default
            #line hidden
            
            #line 39 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 39 ""
            this.Write(" ...\");\n                var ");
            
            #line default
            #line hidden
            
            #line 40 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 40 ""
            this.Write("Response = await ");
            
            #line default
            #line hidden
            
            #line 40 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassName ));
            
            #line default
            #line hidden
            
            #line 40 ""
            this.Write(".GetAllAsync();\n\n                _logger.LogInformation($\"");
            
            #line default
            #line hidden
            
            #line 42 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 42 ""
            this.Write(" loaded successfully!\");\n                dispatcher.Dispatch(new Load");
            
            #line default
            #line hidden
            
            #line 43 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 43 ""
            this.Write("SuccessAction(");
            
            #line default
            #line hidden
            
            #line 43 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 43 ""
            this.Write("Response));\n            }\n            catch (Exception e)\n            {\n         " +
                    "       _logger.LogError($\"Error loading ");
            
            #line default
            #line hidden
            
            #line 47 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 47 ""
            this.Write(", reason: {e.Message}\");\n                dispatcher.Dispatch(new Load");
            
            #line default
            #line hidden
            
            #line 48 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( entityClassNamePlural ));
            
            #line default
            #line hidden
            
            #line 48 ""
            this.Write("FailureAction(e.Message));\n            }                \n        }\n ");
            
            #line default
            #line hidden
            
            #line 51 ""
 
    } else { 
 
            
            #line default
            #line hidden
            
            #line 54 ""
            this.Write(@"        // TODO: If you need some effect, you create partial class of this,
        // and write HandleAsync override methods like below.
        // Don't write here because auto-generated files change will be rewritten.
                    
        public override async Task HandleAsync(");
            
            #line default
            #line hidden
            
            #line 58 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( actionClassName ));
            
            #line default
            #line hidden
            
            #line 58 ""
            this.Write(@" action, IDispatcher dispatcher)
        {
        
            try
            {
                // Write code here
                await Task.Run(() => {});
            }
            catch (Exception e)
            {
                _logger.LogError($""{0} raised on ");
            
            #line default
            #line hidden
            
            #line 68 ""
            this.Write(this.ToStringHelper.ToStringWithCulture( actionClassName ));
            
            #line default
            #line hidden
            
            #line 68 ""
            this.Write(", reason: {e.Message}\", e.GetType().Name);\n            }   \n        }\n           " +
                    "     \n");
            
            #line default
            #line hidden
            
            #line 72 ""
 } /* end if */ 
            
            #line default
            #line hidden
            
            #line 73 ""
            this.Write(" \n    }\n}");
            
            #line default
            #line hidden
            return this.GenerationEnvironment.ToString();
        }
        
        public virtual void Initialize() {
        }
    }
    
    public class LoadListEffectTemplateBase {
        
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
