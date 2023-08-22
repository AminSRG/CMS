﻿using CustomerManagementSystem.Application.Customer.Dtos;
using FluentResults;
using MediatR;

namespace CustomerManagementSystem.Application.Customer.Command
{
    public class DeleteCustomerCommand : IRequest<Result<bool>>
    {
        public CustomerDto CustomerDto { get; set; }
    }

}
