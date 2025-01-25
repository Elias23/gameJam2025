using System;
using UnityEngine;

namespace Assets.RequiredField.Scripts
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredFieldAttribute : PropertyAttribute
    {
    }
}