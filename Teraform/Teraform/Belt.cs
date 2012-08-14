using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teraform
{
    public class Belt
    {

        int _selectedBeltIndex;
        int _numberOfBeltItems;
        int _numberOfQuickSlots;
        //TODO belt item array
        //TODO quick slot array

        Belt(int beltSize, int quickSlots)
        {
            _numberOfBeltItems = beltSize;
            _numberOfQuickSlots = quickSlots;
            _selectedBeltIndex = 0;

        }

        public void SelectNextItem()
        {
            int starting_index = _selectedBeltIndex;
            do
            {
                _selectedBeltIndex++;

                if (_selectedBeltIndex >= _numberOfBeltItems)
                    _selectedBeltIndex = 0;

            } while (false && (_selectedBeltIndex != starting_index));
        }

        public void SelectPreviousItem()
        {
            int starting_index = _selectedBeltIndex;
            do
            {
                if (_selectedBeltIndex <= 0)
                    _selectedBeltIndex = _numberOfBeltItems;

                _selectedBeltIndex--;
            } while (false && (_selectedBeltIndex != starting_index));
        }


    }
}
