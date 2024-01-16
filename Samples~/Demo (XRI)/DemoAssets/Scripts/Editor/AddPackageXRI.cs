#if !XRI_2_4_3_OR_NEWER
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;

namespace Ubiq.Samples.Demo.Editor
{
    [InitializeOnLoad]
    public class AddPackageXRI
    {
        static AddPackageXRI()
        {
#if XRI_0_0_0_OR_NEWER
    #if !UBIQ_SILENCEWARNING_XRIVERSION
            Debug.LogWarning(
                "Ubiq sample DemoScene (XRI) requires XRI > 2.4.3, but an" +
                " earlier version is installed. The sample may not work" +
                " correctly. To silence this warning, add the string" +
                " UBIQ_SILENCEWARNING_XRIVERSION to your scripting define" +
                " symbols");
    #endif
#else
            Client.Add("com.unity.xr.interaction.toolkit");
            Debug.Log("Ubiq added XRI to project requirements");
            AssetDatabase.Refresh();
#endif
        }
    }
}
#endif