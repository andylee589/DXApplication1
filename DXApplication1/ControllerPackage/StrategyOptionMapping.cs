using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXApplication1.ControllerPackage
{
    public static class StrategyOptionMapping
    {
        public  static Type manualWeightSetTypeName = typeof(ManualAssignmentPlatformWeightSetMethod);
        public static  Type autoWeightSetTypeName = typeof(AutoAssignmentPlatformWeightSetMethod);

        public static Type randomPlatformAssignTypeName = typeof(RandomPlatformAssignMethod);
        public static Type balancePlatformAssignTypeName = typeof(BalancedPlatformAssignMethod);

        public static Type autoSecondaryItemWeightSetTypeName = typeof(AutoSecondaryItemWeightSetMethod);
        public static Type manualSecondaryItemWeightSetTypeName = typeof(ManualSecoandaryItemWeightSetMethod);
        public static Type equalSecondaryItemWeightSetTypeName = typeof(EqualSecondaryItemWeightSetMethod);

        public static Type equalFactorPriorityTypeName = typeof(EqualFactorPrioritySetMethod);
        public static Type customFactorPriorotyTypeName = typeof(CustomFactorPrioritySetMethod);

        public static Type equalPlatformPriorityTypeName = typeof(EqualPlatformPrioritySetMethod);
        public static Type exactPlatformPriorityTypeName = typeof(ExactPlatformPrioritySetMethod);

        public static Type firstPriorityMeasureTypeName = typeof(FirstPriorityMeasureMethod);


        public static Dictionary<Type, int> PlatformWeightSetTypeDict = new Dictionary<Type,int>
        {
            {
                typeof(ManualAssignmentPlatformWeightSetMethod),0
            },
            {
                typeof(AutoAssignmentPlatformWeightSetMethod),1
            }
        };

        public static Dictionary<int,Type> PlatformWeightSetIndexDict = new Dictionary<int,Type>
        {
            {
                0,typeof(ManualAssignmentPlatformWeightSetMethod)
            },
            {
                1, typeof(AutoAssignmentPlatformWeightSetMethod)
            }
        };


        public static Dictionary<Type, int> PlatformAssignTypeDict = new Dictionary<Type,int>
        {
            {
                typeof(RandomPlatformAssignMethod),0
            },
            {
                typeof(BalancedPlatformAssignMethod),1
            }
        };

        public static Dictionary<int, Type> PlatformAssignIndexDict = new Dictionary<int,Type>
        {
            {
               0, typeof(RandomPlatformAssignMethod)
            },
            {
                1,typeof(BalancedPlatformAssignMethod)
            }
        };



        public static Dictionary<Type, int> SecondaryItemWeightSetTypeDict = new Dictionary<Type, int>
        {
             // we used for auto assignment platform weight set and don't have radio group in UI,so set -1 to autoSecondaryItemWeight method
            {
                typeof(AutoSecondaryItemWeightSetMethod),-1
            },
             {
                typeof(EqualSecondaryItemWeightSetMethod),0
            },
            {
                typeof(ManualSecoandaryItemWeightSetMethod),1
            },
           
        };

        public static Dictionary<int, Type> SecondaryItemWeightSetIndexDict = new Dictionary<int, Type>
        {
            
            {
               -1, typeof(AutoSecondaryItemWeightSetMethod)
            },
            {
               0, typeof(EqualSecondaryItemWeightSetMethod)
            },
            {
                1,typeof(ManualSecoandaryItemWeightSetMethod)
            }
        };

        public static Dictionary<Type, int> FactorPriorityTypeDict = new Dictionary<Type, int>
        {
             {
                typeof(EqualFactorPrioritySetMethod),0
            },
            {
                typeof(CustomFactorPrioritySetMethod),1
            }
        };

        public static Dictionary<int, Type> FactorPriorityIndexDict = new Dictionary<int, Type>
        {
            {
               0, typeof(EqualFactorPrioritySetMethod)
            },
            {
                1,typeof(CustomFactorPrioritySetMethod)
            }
        };

        public static Dictionary<Type, int> PlatformPriorityTypeDict = new Dictionary<Type, int> 
        {
            {
                typeof(EqualPlatformPrioritySetMethod),0
            },
            {
                typeof(ExactPlatformPrioritySetMethod),1
            }
        };
        
         public static Dictionary<int, Type> PlatformPriorityIndexDict = new Dictionary<int, Type>
        {
            {
               0, typeof(EqualPlatformPrioritySetMethod)
            },
            {
                1,typeof(ExactPlatformPrioritySetMethod)
            }
        };

         public static Dictionary<Type, int> PriorityMeasureTypeDict = new Dictionary<Type, int>
         {
             {
                 typeof(FirstPriorityMeasureMethod),0
             },
         };

         public static Dictionary<int, Type> PriorityMeasureIndexDict = new Dictionary<int, Type>
        {
            {
               0, typeof(FirstPriorityMeasureMethod)
            },
           
        };

    }
}
