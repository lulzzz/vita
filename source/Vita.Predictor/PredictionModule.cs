using Autofac;
using Vita.Contracts;
using Vita.Predictor.TextClassifiers;

namespace Vita.Predictor
{
    public class PredictionModule  : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TextClassifier>()
                .As<ITextClassifier>()
                .SingleInstance()
                .WithParameter((info, context) => info.Name == "companies", (info, context) => context.Resolve<IRepository<Company>>().GetAll())
                .WithParameter((info, context) => info.Name == "localities", (info, context) => context.Resolve<IRepository<Locality>>().GetAll())
                .WithParameter((info, context) => info.Name == "classifiers", (info, context) => context.Resolve<IRepository<Classifier>>().GetAll())
                ;

            builder.RegisterType<Predict>().As<IPredict>();
        }
    }
}
