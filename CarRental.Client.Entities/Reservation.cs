using System;
using Core.Common.Core;

namespace CarRental.Client.Entities
{
    public class Reservation : ObjectBase
    {
        int _ReservationId;
        int _AccountId;
        int _CarId;
        DateTime _RentalDate;
        DateTime _ReturnDate;

        public int ReservationId
        {

            get { return _ReservationId; }

            set
            {
                if (_ReservationId != value)
                {
                    _ReservationId = value;
                    OnPropertyChanged (() => _ReservationId);
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

        public DateTime RentalDate
        {
            get { return _RentalDate; }

            set
            {
                if (_RentalDate != value)
                {

                    _RentalDate = value;
                    OnPropertyChanged (() => _RentalDate);
                }
            }
        }

        public DateTime ReturnDate
        {

            get { return _ReturnDate; }

            set
            {
                if (_ReturnDate != value)
                {
                    _ReturnDate = value;
                    OnPropertyChanged (() => _ReturnDate);
                }
            }

        }
    }
}

