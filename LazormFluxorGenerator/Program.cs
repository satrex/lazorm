using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace LazormFluxorGenerator
{
    public class Generator
    {
        public static void Run(string entity, string outDir, string namespaceText )
        {
            if (string.IsNullOrWhiteSpace(entity))
                throw new ArgumentException("Entity Name must be specified.");
            if (string.IsNullOrWhiteSpace(outDir))
                outDir = Environment.CurrentDirectory;
            Environment.CurrentDirectory = outDir;

            var pluralizer = new Pluralize.NET.Pluralizer();
            GeneratorContext context = new GeneratorContext()
            {
                EntityClassName = pluralizer.Singularize(entity.ToPascalCase()),
                SchemaName = pluralizer.Pluralize(entity.ToPascalCase()),
                NameSpace = namespaceText
            };

            GeneratorContext contextForList = new GeneratorContext()
            {
                EntityClassName = pluralizer.Singularize(entity.ToPascalCase()),
                SchemaName = pluralizer.Pluralize(entity.ToPascalCase()),
                NameSpace = namespaceText
            };

            // Create Once - Static files
            #region RootState
            Directory.CreateDirectory("Store/States");
            RootStateTemplate rootStateTemplate = new RootStateTemplate(context);
            string rootStatePageContent = rootStateTemplate.TransformText();
            File.WriteAllText("Store/States/RootState.cs", rootStatePageContent);
            #endregion
            
            #region Features/Share
            Directory.CreateDirectory("Store/Features/Share");
            FailureActionTemplate failureAction = new FailureActionTemplate(context);
            string failureActionpageContent = failureAction.TransformText();
            File.WriteAllText("Store/Features/Share/FailureAction.cs", failureActionpageContent);
            #endregion

            // Create for each Entities

            #region States
            Directory.CreateDirectory("Store/States");
            StateTemplate stateTemplate = new StateTemplate(context);
            string statePageContent = stateTemplate.TransformText();
            File.WriteAllText($"Store/States/{context.SchemaName}State.cs", statePageContent);
            #endregion

            #region Features
            Directory.CreateDirectory("Store/Features/");
            FeatureTemplate feature = new FeatureTemplate(context: context);
            string featurePageContent = feature.TransformText();
            File.WriteAllText($"Store/Features/{context.SchemaName}Feature.cs",
                featurePageContent);
            #endregion

            // Create for each Entities/CrudKind
            string[] crudKind = new[] { "Create", "Load", "Update", "Delete" };
            foreach (var crud in crudKind)
            {
                context.CrudKind = crud;
                contextForList.CrudKind = crud;

                #region Reducers
                if (context.CrudKind == "Load")
                {
                    LoadListReducerTemplate listReducer = new LoadListReducerTemplate(context: context);
                    string listReducerPageContent = listReducer.TransformText();
                    Directory.CreateDirectory($"Store/Features/{context.EntityClassName}UseCase/Reducers/");
                    File.WriteAllText($"Store/Features/{context.EntityClassName}UseCase/Reducers/{context.CrudKind}{context.SchemaName}Reducer.cs", listReducerPageContent);

                    ReducerTemplate reducerTemplate = new ReducerTemplate(context: context);
                    string reducerPageContent = reducerTemplate.TransformText();
                    File.WriteAllText($"Store/Features/{context.EntityClassName}UseCase/Reducers/{context.CrudKind}{context.EntityClassName}Reducer.cs", reducerPageContent);
                } else {
                    ReducerTemplate reducerTemplate = new ReducerTemplate(context: context);
                    string reducerPageContent = reducerTemplate.TransformText();
                    Directory.CreateDirectory($"Store/Features/{context.EntityClassName}UseCase/Reducers/");
                    File.WriteAllText($"Store/Features/{context.EntityClassName}UseCase/Reducers/{context.CrudKind}{context.EntityClassName}Reducer.cs", reducerPageContent);
                    #endregion
                }
                // Create for each Entities/CrudKind/Try-Success-Failure
                var actionKinds = new string[]
                {
                    "",
                    "Success",
                    "Failure"
                };
                foreach (var actionKind in actionKinds)
                {
                    ActionContext actionContext = new ActionContext()
                    {   
                        ActionKind = actionKind,
                        CrudKind = context.CrudKind,
                        EntityClassName = context.EntityClassName,
                        SchemaName = context.SchemaName,
                        NameSpace = context.NameSpace
                    };

                    #region Effect
                    if (actionContext.CrudKind == "Load")
                    {
                        LoadListEffectTemplate listEffect = new LoadListEffectTemplate(context: actionContext);
                        string effectPageContent = listEffect.TransformText();
                        Directory.CreateDirectory($"Store/Features/{context.EntityClassName}UseCase/Effects/");
                        File.WriteAllText($"Store/Features/{context.EntityClassName}UseCase/Effects/{context.CrudKind}{context.SchemaName}{actionKind}Effect.cs"
                          , effectPageContent);


                        EffectTemplate detailEffect = new EffectTemplate(context: actionContext);
                        string detailEffectPageContent = detailEffect.TransformText();
                        Directory.CreateDirectory($"Store/Features/{context.EntityClassName}UseCase/Effects/");
                        File.WriteAllText($"Store/Features/{context.EntityClassName}UseCase/Effects/{context.CrudKind}{context.EntityClassName}{actionKind}Effect.cs"
                            , detailEffectPageContent);
                    }
                    else { 
                        EffectTemplate effect = new EffectTemplate(context: actionContext);
                        string effectPageContent = effect.TransformText();
                        Directory.CreateDirectory($"Store/Features/{context.EntityClassName}UseCase/Effects/");
                        File.WriteAllText($"Store/Features/{context.EntityClassName}UseCase/Effects/{context.CrudKind}{context.EntityClassName}{actionKind}Effect.cs"
                            , effectPageContent);
                    }
                    #endregion

                    #region Actions
                    // TODO: Add reference for failure action - done
                    if (actionContext.CrudKind == "Load")
                    {
                        // List and Detail

                        LoadListActionTemplate listAction = new LoadListActionTemplate(context: actionContext);
                        string actionPageContent = listAction.TransformText();
                        Directory.CreateDirectory($"Store/Features/{context.EntityClassName}UseCase/Actions/{context.CrudKind}{context.SchemaName}/");
                        File.WriteAllText($"Store/Features/{context.EntityClassName}UseCase/Actions/{context.CrudKind}{context.SchemaName}/{context.CrudKind}{context.SchemaName}{actionKind}Action.cs", actionPageContent);

                        ActionTemplate detailAction = new ActionTemplate(context: actionContext);
                        string detailActionPageContent = detailAction.TransformText();
                        Directory.CreateDirectory($"Store/Features/{context.EntityClassName}UseCase/Actions/{context.CrudKind}{context.EntityClassName}Detail/"); 
                        File.WriteAllText($"Store/Features/{context.EntityClassName}UseCase/Actions/{context.CrudKind}{context.EntityClassName}Detail/{context.CrudKind}{context.EntityClassName}{actionKind}Action.cs", detailActionPageContent);
                    }
                    else
                    {
                        ActionTemplate action = new ActionTemplate(context: actionContext);
                        string actionPageContent = action.TransformText();
                        Directory.CreateDirectory($"Store/Features/{context.EntityClassName}UseCase/Actions/{context.CrudKind}{context.EntityClassName}/");
                        File.WriteAllText($"Store/Features/{context.EntityClassName}UseCase/Actions/{context.CrudKind}{context.EntityClassName}/{context.CrudKind}{context.EntityClassName}{actionKind}Action.cs", actionPageContent);
                    }


                    #endregion
                }

            }


        }
    }
}
