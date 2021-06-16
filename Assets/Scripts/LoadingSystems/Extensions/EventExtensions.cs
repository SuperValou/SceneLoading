using System;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Extensions
{
    internal static class EventExtensions
    {
        internal static void SafeInvoke<TEventArg>(this Action<TEventArg> eventToInvoke, TEventArg eventArg)
        {
            if (eventToInvoke == null)
            {
                return;
            }

            var invocationList = eventToInvoke.GetInvocationList();
            foreach (var delegateToInvoke in invocationList)
            {
                if (delegateToInvoke == null)
                {
                    Debug.LogError($"Unable to invoke null delegate registered on event '{eventToInvoke.Method.Name}'.");
                    continue;
                }

                var castedDelegate = delegateToInvoke as Action<TEventArg>;
                if (castedDelegate == null)
                {
                    Debug.LogError($"Method '{delegateToInvoke.Method.Name}' of object '{delegateToInvoke.Target}' is registered on event {eventToInvoke.Method.Name}, " +
                                   $"but it doesn't take 1 parameter of type '{typeof(TEventArg).Name}'.");
                    continue;
                }

                try
                {
                    castedDelegate.Invoke(eventArg);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Exception occured during event invocation of method '{castedDelegate.Method.Name}' of object '{castedDelegate.Target}': {e}");
                }
            }
        }

        internal static void SafeInvoke<TEventArg1, TEventArg2>(this Action<TEventArg1, TEventArg2> eventToInvoke,
            TEventArg1 eventArg1, TEventArg2 eventArg2)
        {
            if (eventToInvoke == null)
            {
                return;
            }

            var invocationList = eventToInvoke.GetInvocationList();
            foreach (var delegateToInvoke in invocationList)
            {
                if (delegateToInvoke == null)
                {
                    Debug.LogError($"Unable to invoke null delegate registered on event '{eventToInvoke.Method.Name}'.");
                    continue;
                }

                var castedDelegate = delegateToInvoke as Action<TEventArg1, TEventArg2>;
                if (castedDelegate == null)
                {
                    Debug.LogError($"Method '{delegateToInvoke.Method.Name}' of object '{delegateToInvoke.Target}' is registered on event {eventToInvoke.Method.Name}, " +
                                   $"but it doesn't take 2 parameters of type '{typeof(TEventArg1).Name}' and '{typeof(TEventArg2).Name}'.");
                    continue;
                }

                try
                {
                    castedDelegate.Invoke(eventArg1, eventArg2);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Exception occured during invocation of method '{castedDelegate.Method.Name}' of object '{castedDelegate.Target}' (registered on event '{eventToInvoke.Method.Name}'): {e}");
                }
            }

        }

        internal static void SafeInvoke<TEventArg1, TEventArg2, TEventArg3>(this Action<TEventArg1, TEventArg2, TEventArg3> eventToInvoke,
            TEventArg1 eventArg1, TEventArg2 eventArg2, TEventArg3 eventArg3)
        {
            if (eventToInvoke == null)
            {
                return;
            }

            var invocationList = eventToInvoke.GetInvocationList();
            foreach (var delegateToInvoke in invocationList)
            {
                if (delegateToInvoke == null)
                {
                    Debug.LogError($"Unable to invoke null delegate registered on event '{eventToInvoke.Method.Name}'.");
                    continue;
                }

                var castedDelegate = delegateToInvoke as Action<TEventArg1, TEventArg2, TEventArg3>;
                if (castedDelegate == null)
                {
                    Debug.LogError($"Method '{delegateToInvoke.Method.Name}' of object '{delegateToInvoke.Target}' is registered on event {eventToInvoke.Method.Name}, " +
                                   $"but it doesn't take 3 parameters of type '{typeof(TEventArg1).Name}', '{typeof(TEventArg2).Name}', and '{typeof(TEventArg3)}'.");
                    continue;
                }

                try
                {
                    castedDelegate.Invoke(eventArg1, eventArg2, eventArg3);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Exception occured during invocation of method '{castedDelegate.Method.Name}' of object '{castedDelegate.Target}' (registered on event '{eventToInvoke.Method.Name}'): {e}");
                }
            }
        }
    }
}