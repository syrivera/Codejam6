using System.Collections.Immutable;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace CodeJam5b.Functions.Tests.TestUtilities;

internal sealed class FakeFunctionContext : FunctionContext
{
    private IServiceProvider _services;

    public FakeFunctionContext(IServiceProvider? services = null)
    {
        _services = services ?? new ServiceCollection().BuildServiceProvider();
        Features = new FakeInvocationFeatures();
        FunctionDefinition = new FakeFunctionDefinition();
        BindingContext = new FakeBindingContext();
        TraceContext = new FakeTraceContext();
        InvocationId = Guid.NewGuid().ToString();
        Items = new Dictionary<object, object>();
    }

    public override string InvocationId { get; }
    public override string FunctionId => "FakeFunctionId";

    public override TraceContext TraceContext { get; }
    public override BindingContext BindingContext { get; }
    public override RetryContext? RetryContext => null;

    public override IServiceProvider InstanceServices
    {
        get => _services;
        set => _services = value;
    }

    public override FunctionDefinition FunctionDefinition { get; }
    public override IInvocationFeatures Features { get; }

    public override IDictionary<object, object> Items { get; set; }

    private sealed class FakeInvocationFeatures : IInvocationFeatures
    {
        private readonly Dictionary<Type, object?> _features = new();

        T IInvocationFeatures.Get<T>()
        {
            return _features.TryGetValue(typeof(T), out var value) ? (T)value! : default!;
        }

        void IInvocationFeatures.Set<T>(T instance)
        {
            if (instance is null)
            {
                _features.Remove(typeof(T));
                return;
            }

            _features[typeof(T)] = instance;
        }

        public IEnumerator<KeyValuePair<Type, object?>> GetEnumerator() => _features.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private sealed class FakeBindingContext : BindingContext
    {
        public override IReadOnlyDictionary<string, object> BindingData { get; } =
            new Dictionary<string, object>();
    }

    private sealed class FakeTraceContext : TraceContext
    {
        public override string TraceParent => string.Empty;
        public override string TraceState => string.Empty;
    }

    private sealed class FakeFunctionDefinition : FunctionDefinition
    {
        public override string PathToAssembly => string.Empty;
        public override string EntryPoint => string.Empty;
        public override string Id => "Fake";
        public override string Name => "Fake";

        public override IImmutableDictionary<string, BindingMetadata> InputBindings { get; } =
            ImmutableDictionary<string, BindingMetadata>.Empty;

        public override IImmutableDictionary<string, BindingMetadata> OutputBindings { get; } =
            ImmutableDictionary<string, BindingMetadata>.Empty;

        public override ImmutableArray<FunctionParameter> Parameters { get; } =
            ImmutableArray<FunctionParameter>.Empty;
    }
}
