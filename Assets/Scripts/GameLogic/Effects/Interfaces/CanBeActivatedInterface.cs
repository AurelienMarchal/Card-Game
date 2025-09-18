using System.Collections.Generic;


namespace GameLogic
{
    namespace GameEffect
    {
        public interface CanBeActivatedInterface
        {

            public bool CanBeActivated();

            protected void Activate();

            public bool TryToActivate(){
                var result = CanBeActivated();
                if(result){
                    Activate();
                }
                return result;
            }

        }
    }
}