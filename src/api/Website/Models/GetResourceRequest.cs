namespace Website.Models;

using Mediator;
using Microsoft.AspNetCore.Http;

public record GetResourceRequest(
    string CaptchaToken,
    string ResourceLink) : IRequest<IResult>;