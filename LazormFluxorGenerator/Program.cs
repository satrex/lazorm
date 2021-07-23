using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace LazormFluxorGenerator
{
    public class Generator
    {
        public static void Run(string entity, string outDir )
        {
            if (string.IsNullOrWhiteSpace(entity))
                throw new ArgumentException("Entity Name must be specified.");
            if (string.IsNullOrWhiteSpace(outDir))
                outDir = Environment.CurrentDirectory;
            Environment.CurrentDirectory = outDir;

            var pluralizer = new Pluralize.NET.Pluralizer();
            GeneratorContext context = new GeneratorContext()
            {
                EntityClassName = entity
            };
            context.SchemaName = pluralizer.Pluralize(context.EntityClassName);

            // Create Once - Static files
            #region RootState
            Directory.CreateDirectory("Fluxor/Store/States");
            RootStateTemplate rootStateTemplate = new RootStateTemplate(context);
            string rootStatePageContent = rootStateTemplate.TransformText();
            File.WriteAllText("Fluxor/Store/States/RootState.cs", rootStatePageContent);
            #endregion
            
            #region Features/Share
            Directory.CreateDirectory("Fluxor/Store/Features/Share");
            FailureActionTemplate failureAction = new FailureActionTemplate();
            string failureActionpageContent = failureAction.TransformText();
            File.WriteAllText("Fluxor/Store/Features/Share/FailureAction.cs", failureActionpageContent);
            #endregion

            // Create for each Entities

            #region States
            Directory.CreateDirectory("Fluxor/Store/States");
            StateTemplate stateTemplate = new StateTemplate(context);
            string statePageContent = stateTemplate.TransformText();
            File.WriteAllText(string.Format("Fluxor/Store/States/{0}State.cs",context.SchemaName ), statePageContent);
            #endregion

            #region Features
            Directory.CreateDirectory("Fluxor/Store/Features/");
            FeatureTemplate feature = new FeatureTemplate(context: context);
            string featurePageContent = feature.TransformText();
            File.WriteAllText(string.Format("Fluxor/Store/Features/{0}Feature.cs", context.SchemaName),
                featurePageContent);
            #endregion

            // Create for each Entities/CrudKind
            string[] crudKind = new[] { "Create", "Load", "Update", "Delete" };
            foreach (var crud in crudKind)
            {
                context.CrudKind = crud;

                #region Reducers
                // TODO: Add reference for states- done
                // TODO: Add reference for Lazorm Entity - done
                if (context.CrudKind == "Load")
                {
                    LoadListReducerTemplate listReducer = new LoadListReducerTemplate(context: context);
                    string listReducerPageContent = listReducer.TransformText();
                    Directory.CreateDirectory(string.Format("Fluxor/Store/Features/{1}UseCase/Reducers/",
                        context.SchemaName, context.EntityClassName));
                    File.WriteAllText(string.Format("Fluxor/Store/Features/{1}UseCase/Reducers/{2}{0}Reducer.cs",
                     context.SchemaName, context.EntityClassName, context.CrudKind), listReducerPageContent);

                    ReducerTemplate reducerTemplate = new ReducerTemplate(context: context);
                    string reducerPageContent = reducerTemplate.TransformText();
                    File.WriteAllText(string.Format("Fluxor/Store/Features/{2}UseCase/Reducers/{1}{2}Reducer.cs",
                        context.SchemaName, context.CrudKind, context.EntityClassName), reducerPageContent);
                } else {
                    ReducerTemplate reducerTemplate = new ReducerTemplate(context: context);
                    string reducerPageContent = reducerTemplate.TransformText();
                    Directory.CreateDirectory(string.Format("Fluxor/Store/Features/{1}UseCase/Reducers/",
    context.SchemaName, context.EntityClassName));
                    File.WriteAllText(string.Format("Fluxor/Store/Features/{2}UseCase/Reducers/{1}{2}Reducer.cs",
                        context.SchemaName, context.CrudKind, context.EntityClassName), reducerPageContent);
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
                        SchemaName = context.SchemaName
                    };

                    #region Effect
                    // TODO: Add reference of Fluxor - done 
                    // TODO: Add reference of Lazorm Entity - done 
                    // TODO: Fix indents - done
                    if (actionContext.CrudKind == "Load")
                    {
                        LoadListEffectTemplate listEffect = new LoadListEffectTemplate(context: actionContext);
                        string effectPageContent = listEffect.TransformText();
                        Directory.CreateDirectory(string.Format("Fluxor/Store/Features/{1}UseCase/Effects/", 
                            context.SchemaName, context.EntityClassName));
                        File.WriteAllText(string.Format("Fluxor/Store/Features/{1}UseCase/Effects/{2}{0}{3}Effect.cs",
                         context.SchemaName, context.EntityClassName, context.CrudKind, actionKind), effectPageContent);


                        EffectTemplate detailEffect = new EffectTemplate(context: actionContext);
                        string detailEffectPageContent = detailEffect.TransformText();
                        Directory.CreateDirectory(string.Format("Fluxor/Store/Features/{1}UseCase/Effects/", 
                            context.SchemaName, context.EntityClassName));
                        File.WriteAllText(string.Format("Fluxor/Store/Features/{0}UseCase/Effects/{1}{0}{3}Effect.cs", 
                            context.EntityClassName, context.CrudKind, context.SchemaName, actionKind), detailEffectPageContent);
                    }
                    else { 
                        EffectTemplate effect = new EffectTemplate(context: actionContext);
                        string effectPageContent = effect.TransformText();
                        Directory.CreateDirectory(string.Format("Fluxor/Store/Features/{1}UseCase/Effects/",
                            context.SchemaName, context.EntityClassName));
                        File.WriteAllText(string.Format("Fluxor/Store/Features/{0}UseCase/Effects/{1}{0}{3}Effect.cs",
                            context.EntityClassName, context.CrudKind,
                            context.SchemaName, actionKind), effectPageContent);
                    }
                    #endregion

                    #region Actions
                    // TODO: Add reference for failure action - done
                    if(actionContext.CrudKind == "Load")
                    {
                        // List and Detail
                        LoadListActionTemplate listAction = new LoadListActionTemplate(context: actionContext);
                        string actionPageContent = listAction.TransformText();
                        Directory.CreateDirectory(string.Format("Fluxor/Store/Features/{2}UseCase/Actions/{1}{0}/",
                            context.SchemaName, context.CrudKind, context.EntityClassName));
                        File.WriteAllText(string.Format("Fluxor/Store/Features/{2}UseCase/Actions/{1}{0}/{1}{0}{3}Action.cs",
                            context.SchemaName, context.CrudKind, context.EntityClassName, actionKind), actionPageContent);

                        ActionTemplate detailAction = new ActionTemplate(context: actionContext);
                        string detailActionPageContent = detailAction.TransformText();
                        Directory.CreateDirectory(string.Format("Fluxor/Store/Features/{2}UseCase/Actions/{1}{2}Detail/",
                           context.SchemaName, context.CrudKind, context.EntityClassName)); 
                        File.WriteAllText(string.Format("Fluxor/Store/Features/{2}UseCase/Actions/{1}{2}Detail/{1}{2}{3}Action.cs",
                            context.SchemaName, context.CrudKind, context.EntityClassName, actionKind), detailActionPageContent);
                    }
                    else
                    {
                        ActionTemplate action = new ActionTemplate(context: actionContext);
                        string actionPageContent = action.TransformText();
                        Directory.CreateDirectory(string.Format("Fluxor/Store/Features/{2}UseCase/Actions/{1}{2}/",
                            context.SchemaName, context.CrudKind, context.EntityClassName));
                        File.WriteAllText(string.Format("Fluxor/Store/Features/{2}UseCase/Actions/{1}{2}/{1}{2}{3}Action.cs",
                            context.SchemaName, context.CrudKind, context.EntityClassName, actionKind), actionPageContent);
                    }


                    #endregion
                }

            }


        }
    }
}
