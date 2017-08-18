﻿using System;
using System.Collections.Generic;
using System.Configuration;
using Cielo.Configuration;
using Cielo.Enums;
using Cielo.Request.Entites;
using Cielo.Request.Entites.Common;
using Cielo.Responses;
using Cielo.Responses.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Cielo.Test
{
    [TestFixture]
    public class CieloServiceTests
    {
        [Test]
        public void CreateCreditCardRequest_ShouldReturnNewTransactionResponse()
        {
            CieloService cieloService = new CieloService();

            Customer customer = new Customer("John Doe");


            CreditCard creditCard = new CreditCard("0000.0000.0000.0001",
                                            "John Doe",
                                            new CardExpiration(2017, 9), "123", CardBrand.Visa);

            Payment payment = new Payment(PaymentType.CreditCard, 380.2m, 1, "", creditCard: creditCard);

            var transaction = new TransactionRequest("14421", customer, payment);

            var response = cieloService.CreateTransaction(transaction);

            response.Should().NotBeNull();
            response.Should().BeOfType<NewTransactionResponse>();
            response.Tid.Should().NotBeEmpty();
            response.PaymentId.Should().NotBeEmpty();
            response.MerchantOrderId.Should().NotBeEmpty();
            response.ProofOfSale.Should().NotBeEmpty();
            response.AuthorizationCode.Should().NotBeEmpty();
            response.Status.Should().BeOfType<Status>();
            response.ReturnCode.Should().BeOfType<ReturnCode>();
            response.ReturnMessage.Should().NotBeEmpty();
            response.AuthenticationUrl.Should().BeNullOrEmpty();
        }

        [Test]
        public void CreateDebitCard_ShouldReturnNewTransactionResponse()
        {
            CustomConfiguration configuration = new CustomConfiguration()
            {
                DefaultEndpoint = ConfigurationManager.AppSettings["cielo.endpoint.default"],
                QueryEndpoint = ConfigurationManager.AppSettings["cielo.endpoint.query"],
                MerchantId = ConfigurationManager.AppSettings["cielo.customer.id"],
                MerchantKey = ConfigurationManager.AppSettings["cielo.customer.key"],
                ReturnUrl = "http://localhost" + ConfigurationManager.AppSettings["cielo.return.url"] + "/14421"
            };

            CieloService cieloService = new CieloService(configuration);

            Customer customer = new Customer("John Doe");


            DebitCard debitCard = new DebitCard("0000.0000.0000.0001",
                                            "John Doe",
                                            new CardExpiration(2017, 9), "123", CardBrand.Visa);

            Payment payment = new Payment(PaymentType.DebitCard, 380.2m, 1, "", debitCard: debitCard, returnUrl: configuration.ReturnUrl);

            var transaction = new TransactionRequest("14421", customer, payment);

            var response = cieloService.CreateTransaction(transaction);
            
            response.Should().NotBeNull();
            response.Should().BeOfType<NewTransactionResponse>();
            response.Tid.Should().NotBeEmpty();
            response.PaymentId.Should().NotBeEmpty();
            response.MerchantOrderId.Should().NotBeEmpty();
            response.ProofOfSale.Should().NotBeEmpty();
            response.AuthorizationCode.Should().BeNullOrEmpty();
            response.Status.Should().BeOfType<Status>();
            response.ReturnCode.Should().BeOfType<ReturnCode>();
            response.ReturnMessage.Should().BeNullOrEmpty();
            response.AuthenticationUrl.Should().NotBeNullOrEmpty();
        }
        
        [Test]
        public void CreateRequestWithCardToken_ShouldReturnNewTransactionResponse()
        {
            CieloService cieloService = new CieloService();

            Customer customer = new Customer("John Doe");

            CreditCard creditCard = new CreditCard("ea9b8398-e8bb-4e77-a865-66109fc4563e", "123", CardBrand.Visa);

            Payment payment = new Payment(PaymentType.CreditCard, 380.2m, 1, "", creditCard: creditCard);

            var transaction = new TransactionRequest("14421", customer, payment);

            var response = cieloService.CreateTransaction(transaction);

            response.Should().NotBeNull();
            response.Should().BeOfType<NewTransactionResponse>();
            response.Tid.Should().NotBeEmpty();
            response.PaymentId.Should().NotBeEmpty();
            response.MerchantOrderId.Should().NotBeEmpty();
            response.ProofOfSale.Should().NotBeEmpty();
            response.AuthorizationCode.Should().NotBeEmpty();
            response.Status.Should().BeOfType<Status>();
            response.ReturnCode.Should().BeOfType<ReturnCode>();
            response.ReturnMessage.Should().NotBeEmpty();
        }
        
        [Test]
        public void CancelTransaction_GivenFakeMerchantOrderId_ShouldThrowAnExceptionOfTypeResponseException()
        {
            CieloService cieloService = new CieloService();

            cieloService.Invoking(c => c.CancelTransaction(merchantOrderId: "123123")).ShouldThrow<ResponseException>().WithMessage("Transaction not available to void");
        }

        [Test]
        public void CaptureTransaction_GivenFakePaymentId_ShouldThrowAnExceptionOfTypeResponseException()
        {
            CieloService cieloService = new CieloService();

            cieloService.Invoking(c => c.CaptureTransaction(Guid.Parse("55158bb3-2bb9-4e76-a92b-708b51245f4b"))).ShouldThrow<ResponseException>().WithMessage("Transaction not available to capture");
        }
        
        [Test]
        public void CapturePartialTransaction_GivenFakePaymentId_ShouldThrowAnExceptionOfTypeResponseException()
        {
            CieloService cieloService = new CieloService();

            cieloService.Invoking(c => c.CaptureTransaction(Guid.Parse("55158bb3-2bb9-4e76-a92b-708b51245f4b"), 20.00m)).ShouldThrow<ResponseException>().WithMessage("Transaction not available to capture");
        }

        [Test]
        public void CheckTransaction_ShouldReturnCardResponse()
        {
            CieloService cieloService = new CieloService();

            var response = cieloService.CheckTransaction(merchantOrderId: "14421");

            response.Should().NotBeNull();
            response.Should().BeOfType<CheckTransactionResponse>();
            response.ReasonMessage.Should().NotBeEmpty();
            response.Payments.Should().BeOfType<List<PaymentResponse>>();
            response.Payments.Count.Should().BeGreaterThan(0);
        }

        [Test]
        public void SaveCard_ShouldReturnCheckTransactionResponse()
        {
            CieloService cieloService = new CieloService();

            CreditCardRequest request = new CreditCardRequest("John Doe", "0000.0000.0000.0004", "John Doe", new CardExpiration(2020, 8), CardBrand.MasterCard);

            var response = cieloService.SaveCard(request);

            response.Should().NotBeNull();
            response.Should().BeOfType<CardResponse>();
            response.CardToken.Should().NotBeEmpty();
        }
    }
}
