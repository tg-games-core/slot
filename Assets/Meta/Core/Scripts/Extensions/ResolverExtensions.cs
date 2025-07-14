using Core.UI.MVC;
using Core.UI.MVC.Interface;
using VContainer;

namespace Core
{
    public static class ResolverExtensions
    {
        public static TController ResolvePresenter<TController, TModel>(this IObjectResolver resolver, IView view)
            where TModel : Model, new()
            where TController : IController, new() 
        {
            var model = new TModel();
            resolver.Inject(model);

            var controller = new TController();
            resolver.Inject(controller);
            controller.Build(view, model);

            return controller;
        }
    }
}