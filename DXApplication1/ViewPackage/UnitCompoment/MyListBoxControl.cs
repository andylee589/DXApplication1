using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;

namespace DXApplication1.DataModelPackage
{
    public class MyListBoxControl : ListBoxControl
    {
        public delegate bool BeforeSelectEventHandler(object sender, BeforeSelectEventArgs e);
        public event BeforeSelectEventHandler BeforeSelect;

        public MyListBoxControl():base()
        {
           
        }

        protected override ListBoxControlHandler CreateHandler()
        {
            return new MyListBoxControlHandler(this);
        }

        public bool RaiseBeforeSelectEvent(int lastIndex, int currentIndex)
        {
            BeforeSelectEventArgs e = new BeforeSelectEventArgs(lastIndex,currentIndex);
          return   BeforeSelect(this, e);
        }
    }

    public class MyListBoxControlHandler : ListBoxControlHandler
    {
        public MyListBoxControlHandler(BaseListBoxControl owner)
            : base(owner)
        {
            
        }

        protected override ListBoxControlHandler.ListBoxControlState CreateState(ListBoxControlHandler.HandlerState state)
        {
            if (state == HandlerState.Default || state == HandlerState.SingleSelect)
            {
                return new MySingleSelectState(this);
            }
            else
            {
                return base.CreateState(state);
            }
        }

        public bool raiseBeforeSelectEvent(int lastIndex,int currentIndex)
        {
           return (this.OwnerControl as MyListBoxControl).RaiseBeforeSelectEvent(lastIndex ,currentIndex);
        }

    }

    public class MySingleSelectState : DevExpress.XtraEditors.ListBoxControlHandler.SingleSelectState
    {
        public MySingleSelectState(ListBoxControlHandler handler)
            : base(handler)
        {

        }

        protected override int FocusedIndex
        {
            get
            {
                return base.FocusedIndex;
            }
            set
            {
                if (base.FocusedIndex != value)
                {
                    if ((this.Handler as MyListBoxControlHandler).raiseBeforeSelectEvent(base.FocusedIndex, value))
                    {
                        base.FocusedIndex = value;
                    }
                    
                }
                
            }
        }   
    }

     public class BeforeSelectEventArgs :EventArgs
        {
            private int lastIndex;
            private int currentIndex;
            public BeforeSelectEventArgs(int lastIndex,int currentIndex)
                : base()
            {
                this.lastIndex = lastIndex;
                this.currentIndex = currentIndex;
            }

            public int LastIndex
            {
                get
                {
                    return lastIndex;
                }
            }

            public int CurrentIndex
            {
                get
                {
                    return currentIndex;
                }
                set
                {
                    currentIndex = value;
                }
            }

        }
}
