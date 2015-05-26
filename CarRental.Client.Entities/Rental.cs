using System;
using Core.Common.Core;
namespace CarRental.Client.Entities
{
    public class Rental : ObjectBase
    {
        int _RentalId;
        int _AccountId;
        int _CarId;
        DateTime _DateRented;
        DateTime _DateReturned;
        DateTime? _DateDue;

        public int RentalId
        {

            get { return _RentalId; }

            set
            {
                if (_RentalId != value)
                {
                    _RentalId = value;
                    OnPropertyChanged (() => _RentalId);
                }
            }
        }

        public int AccountId
        {
            get { return _AccountId; }

            set
            {
                if (_AccountId != value)
                {
                    _AccountId = value;
                    OnPropertyChanged (() => _AccountId);
                }
            }

        }

        public int CarId
        {
            get { return _CarId; }

            set
            {
                if (_CarId != value)
                {

                    _CarId = value;
                    OnPropertyChanged (() => _CarId);
                }
            }

        }

        public DateTime DateRented
        {
            get { return _DateRented; }

            set
            {
                if (_DateRented != value)
                {

                    _DateRented = value;
                    OnPropertyChanged (() => _DateRented);
                }
            }
        }

        public DateTime DateReturned
        {

            get { return _DateReturned; }

            set
            {
                if (_DateReturned != value)
                {
                    _DateReturned = value;
                    OnPropertyChanged (() => _DateReturned);
                }
            }
        }

        public DateTime? DateDue
        {
            get { return _DateDue; }

            set
            {
                if (_DateDue != value)
                {
                    _DateDue = value;
                    OnPropertyChanged (() => _DateDue);
                }
            }

        }
    }
}
