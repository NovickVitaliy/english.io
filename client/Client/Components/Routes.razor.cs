using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Client.Components;

public partial class Routes : ComponentBase
{
    public readonly Assembly[] AdditionalAssemblies = [
        typeof(Authentication.Marker).Assembly
    ];
}