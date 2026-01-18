//this checks if the player actually owns the game and stops piracy at a very basic level
//biblically accurate entitlement check :O
//you obviously need the Meta XR Core/Platform sdks for this to work
using Oculus.Platform;
using UnityEngine;

#if !UNITY_EDITOR
using System.Threading;
using UnityEngine.Diagnostics;
#endif

public static class MetaEntitlementCheck
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)] //Run before the splashscreen also you dont have to put this in your scene
    /// <summary>
    /// This runs before the splash screen and if you aren't initialized with meta yet it will initialize you
    /// after you are initialized it will check if you are entitled
    /// <summary>
    private static void MetaInit()
    {
        if (!Core.IsInitialized())
        {
            Core.Initialize();

            if (Core.IsInitialized())
            {
                Entitlements.IsUserEntitledToApplication().OnComplete(CheckEntitlement);
            }
        }
    }

    /// <summary>
    /// Checks if you are entitled to the game as it states
    /// </summary>
    /// <param name="msg"></param>
    private static void CheckEntitlement(Message msg)
    {
        if (!msg.IsError)
        {
            Debug.Log($"<color=#ADD8E6>[MetaEntitlementCheck]</color> Hooray! You are entitled to use this application! Meta Message: {msg}");
        }
        else
        {
            Debug.LogError($"<color=#ADD8E6>[MetaEntitlementCheck]</color> You are not entitled to use this application :`(. Error Message: {msg}");
#if !UNITY_EDITOR
            Utils.ForceCrash(ForcedCrashCategory.AccessViolation); //Force crashes the game so cheater can't Application.CancelQuit()
            Thread.Sleep(2147483647); //stops threads for 3 and a half weeks :)
#endif
        }
    }
}