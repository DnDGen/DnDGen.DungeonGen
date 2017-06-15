using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Generators.ContentGenerators;
using DungeonGen.Domain.Generators.Dungeons;
using DungeonGen.Domain.Generators.ExitGenerators;
using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.IoC.Providers;
using Ninject.Activation;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.IoC.Modules
{
    internal class GeneratorsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDungeonGenerator>().To<DungeonGenerator>().WhenInjectedInto<DungeonGeneratorEventDecorator>();
            Bind<IDungeonGenerator>().To<DungeonGeneratorEventDecorator>();

            Bind<JustInTimeFactory>().ToProvider<JustInTimeFactoryProvider>();
            Bind<AreaGeneratorFactory>().To<DomainAreaGeneratorFactory>();

            Bind<ITrapGenerator>().To<TrapGenerator>();
            Bind<ContentsGenerator>().To<DomainContentsGenerator>();
            Bind<PoolGenerator>().To<DomainPoolGenerator>();

            var decorators = new[]
            {
                typeof(AreaGeneratorEventDecorator),
            };

            Decorate<AreaGenerator, CaveGenerator>(AreaTypeConstants.Cave, decorators);
            Decorate<AreaGenerator, ChamberGenerator>(AreaTypeConstants.Chamber, decorators);
            Decorate<AreaGenerator, DoorGenerator>(AreaTypeConstants.Door, decorators);
            Decorate<AreaGenerator, HallGenerator>(AreaTypeConstants.Hall, decorators);
            Decorate<AreaGenerator, RoomGenerator>(AreaTypeConstants.Room, decorators);
            Decorate<AreaGenerator, ParallelPassageGenerator>(SidePassageConstants.ParallelPassage, decorators);
            Decorate<AreaGenerator, SidePassageGenerator>(AreaTypeConstants.SidePassage, decorators);
            Decorate<AreaGenerator, SpecialAreaGenerator>(AreaTypeConstants.Special, decorators);
            Decorate<AreaGenerator, StairsGenerator>(AreaTypeConstants.Stairs, decorators);
            Decorate<AreaGenerator, TurnGenerator>(AreaTypeConstants.Turn, decorators);

            Bind<ExitGenerator>().To<ChamberExitGenerator>().Named(AreaTypeConstants.Chamber);
            Bind<ExitGenerator>().To<RoomExitGenerator>().Named(AreaTypeConstants.Room);
        }

        private void Decorate<S, T>(string name, params Type[] decorators)
            where T : S
        {
            var implementations = new[] { typeof(T) }.Union(decorators).Take(decorators.Length);

            foreach (var implementation in implementations)
            {
                Bind<S>().To(implementation).When(r => Need(implementation, r, name, implementations));
            }

            Bind<S>().To(decorators.Last()).Named(name);
        }

        private bool Need(Type implementation, IRequest request, string name, IEnumerable<Type> implementations)
        {
            var implementationsList = implementations.ToList();
            var depth = implementationsList.Count - implementationsList.IndexOf(implementation);

            return request.Depth == depth && request.ActiveBindings.Any(b => b.Metadata.Name == name);
        }
    }
}
