using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged
{
    /// <summary>
    /// 类级别的NPCA切面
    /// </summary>
    [Serializable, DebuggerNonUserCode, LinesOfCodeAvoided(50),
        AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false),
        MulticastAttributeUsage(MulticastTargets.Class, AllowMultiple = false,
        Inheritance = MulticastInheritance.None, AllowExternalAssemblies = false)]
    public sealed class NPCA_ClassAttribute : TypeLevelAspect, IAspectProvider
    {
        public IEnumerable<AspectInstance> ProvideAspects(object targetElement)
        {
            Type targetType = (Type)targetElement;            

            MemberInfo setMethod = null;
            foreach (PropertyInfo property in
              targetType.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
            {
                if (property.IsDefined(typeof(NotNPC_PropertyAttribute)) == false)
                {
                    setMethod = property.GetSetMethod();

                    if (setMethod != null)
                    {
                        yield return new AspectInstance(setMethod,
                            new NPCA_MethodAttribute(property.Name) as IAspect);
                        // Type genericAspect = typeof(NPCA_MethodAttribute);
                        // Activator.CreateInstance(genericAspect,new object[] { property.Name })
                    }
                }                
            }
        }
    }

}
