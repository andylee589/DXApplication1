using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXApplication1.ControllerPackage
{
    public static class TypeSwitch
    {
        public static Switch<TSource> On<TSource>(TSource value)
        {
            return new Switch<TSource>(value);
        }

        public class Switch<TSource>
        {
            private TSource value;
            private bool handled = false;

            internal Switch(TSource value)
            {
                this.value = value;
            }

            public Switch<TSource> Case<Ttarget>(Action<Ttarget> action) where Ttarget : TSource
            {
                if (!handled)
                {
                    var sourceType = value.GetType();
                    var targetType = typeof(Ttarget);
                    if (targetType.IsAssignableFrom(sourceType))
                    {
                        action((Ttarget)value);
                        handled = true;
                    }
                }

                return this;
            }

            public void Default(Action<TSource> action)
            {
                if (!handled)
                {
                    action(value);
                }
            }
        }
    }
}
