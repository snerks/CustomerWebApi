using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Ploeh.AutoFixture;

namespace CustomerWebApi.UnitTest.AutoFixture
{
    public class MvcCoreControllerCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new System.ArgumentNullException(nameof(fixture));
            }

            fixture
                .Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));

            fixture
                .Behaviors
                .Add(new OmitOnRecursionBehavior());

            fixture.Register<IModelMetadataProvider>(() => new EmptyModelMetadataProvider());
        }
    }
}
