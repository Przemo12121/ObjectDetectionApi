global using Xunit;
global using FluentAssertions;
global using Application.Validators;
global using Application.Requests.Payloads;
global using Domain.SeedWork.Enums;
global using Application.Responses;
global using NSubstitute;
global using FluentValidation.Results;
global using Application.Requests;
global using MediatR;
global using Application.PipelineBehaviors;
global using Domain.AggregateModels.AccessAccountAggregate;
global using Domain.AggregateModels.ProcessedFileAggregate;
global using Application.UnitTests.PipelineBehaviorsTests.DummyRequests;
global using Domain.AggregateModels;
global using Domain.AggregateModels.OriginalFileAggregate;
global using FluentValidation;

