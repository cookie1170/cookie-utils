using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.LowLevel;

namespace CookieUtils
{
    [PublicAPI]
    public static class PlayerLoopUtils
    {
        public static void RemoveSystem(ref PlayerLoopSystem loop, in PlayerLoopSystem system) {
            if (loop.subSystemList == null) return;

            List<PlayerLoopSystem> systems = new(loop.subSystemList);
            for (int i = 0; i < systems.Count; i++) {
                PlayerLoopSystem subSystem = systems[i];
                if (subSystem.type == system.type && subSystem.updateDelegate == system.updateDelegate) {
                    systems.RemoveAt(i);
                    loop.subSystemList = systems.ToArray();
                }
            }

            HandleSubsystemRemove(ref loop, in system);
        }

        private static void HandleSubsystemRemove(ref PlayerLoopSystem loop, in PlayerLoopSystem system) {
            for (int i = 0; i < loop.subSystemList.Length; i++) RemoveSystem(ref loop.subSystemList[i], in system);
        }

        public static bool InsertSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem system, int index) {
            if (loop.type != typeof(T)) return HandleSubsystemInsert<T>(ref loop, in system, index);

            List<PlayerLoopSystem> systemList = new();

            if (loop.subSystemList != null) systemList.AddRange(loop.subSystemList);

            systemList.Insert(index, system);
            loop.subSystemList = systemList.ToArray();

            return true;
        }

        private static bool HandleSubsystemInsert<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem system, int index) {
            if (loop.subSystemList == null) return false;

            for (int i = 0; i < loop.subSystemList.Length; i++) {
                if (!InsertSystem<T>(ref loop.subSystemList[i], in system, index)) continue;

                return true;
            }

            return false;
        }

        public static void PrintLoop(PlayerLoopSystem loop) {
            StringBuilder builder = new();
            builder.AppendLine("Player loop:");

            foreach (PlayerLoopSystem subSystem in loop.subSystemList) {
                builder.AppendLine(subSystem.type.Name);
                HandleSubsystemPrint(loop, ref builder, 1);
            }

            Debug.Log(builder.ToString());
        }

        private static void HandleSubsystemPrint(PlayerLoopSystem loop, ref StringBuilder builder, int depth) {
            if (loop.subSystemList == null) return;

            foreach (PlayerLoopSystem subSystem in loop.subSystemList) {
                for (int i = 0; i < depth * 2; i++)
                    builder.Append(' ');

                builder.AppendLine($"{subSystem.type.Name}");
                HandleSubsystemPrint(subSystem, ref builder, depth + 1);
            }
        }
    }
}