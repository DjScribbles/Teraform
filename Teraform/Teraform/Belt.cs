﻿using System;
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
        Item[] _beltItems;
        Item[] _quickSlotItems;
        //TODO belt item array
        //TODO quick slot array

        public Belt(int beltSize = 10, int quickSlots = 10)
        {
            _numberOfBeltItems = beltSize;
            _numberOfQuickSlots = quickSlots;
            _selectedBeltIndex = 0;
            _beltItems = new Item[_numberOfBeltItems];
            _quickSlotItems = new Item[_numberOfQuickSlots];

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

        public Item GetCurrentBeltItem()
        {
            return _beltItems[_selectedBeltIndex];
        }

        public bool AddBeltItem(Item item, int index)
        {
            if (item == null) return false;

            if (index < _numberOfBeltItems && _beltItems[index] == null)
            {
                _beltItems[index] = item;
                return true;
            }
            return false;
        }

        public bool AddBeltItem(Item item)
        {
            if (item == null) return false;

            int index = 0;
            bool placed = false;
            
            while ((placed == false) && (index < _numberOfBeltItems))
            {
                if (_beltItems[index] == null)
                {
                    _beltItems[index] = item;
                    placed = true;
                }
                index++;
            }
            return placed;
        }
    }
}
