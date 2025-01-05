using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Client;

public partial class Routes : ComponentBase
{
    public readonly Assembly[] AdditionalAssemblies = [
        typeof(Authentication.AuthenticationMarker).Assembly
    ];
}