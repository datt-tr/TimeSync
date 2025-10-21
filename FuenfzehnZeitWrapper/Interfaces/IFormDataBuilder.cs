using FuenfzehnZeitWrapper.Enums;

namespace FuenfzehnZeitWrapper.Interfaces;

public interface IFormDataBuilder
{
  MultipartFormDataContent Build(RequestType type);
}