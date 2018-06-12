using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace MC{
	public static class ComponentValidator {
        public static bool Validate(object obj, string objName) {
            StackFrame frame = new StackFrame(1);
            var type = frame.GetMethod().DeclaringType;
            if (obj==null) {
                UnityEngine.Debug.LogError($"[{type}] {objName} is not assigned on the inspector");	
				UnityEngine.Debug.Break();
                return false;
            }
            return true;
        }   

        public static bool Validate(object[] objs, string objsName){
            StackFrame frame = new StackFrame(1);
            var type = frame.GetMethod().DeclaringType;
            if(objs.Length < 1) {
                UnityEngine.Debug.LogError($"[{type}] There is not a single {objsName} assigned on the inspector");	
                return false;
            }
            return true;
        }
	}
}