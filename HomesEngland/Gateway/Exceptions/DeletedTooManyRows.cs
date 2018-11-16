﻿namespace HomesEngland.Gateway.Exceptions
{
    public class DeletedTooManyRows : System.Exception
    {
        public DeletedTooManyRows() : base("SqlGateway Deleted more the expected amount of data")
        {

        }
    }
}