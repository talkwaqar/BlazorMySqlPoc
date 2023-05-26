using System.ComponentModel.Design;
using System.Net.Http;
using System.Net.Http.Json;
using System.Xml.Linq;
using PersonDemo.Shared.Models;

namespace PersonDemoBlazorUI.Services;

public class PersonService
{
    private HttpService _httpService;

    public PersonService(HttpService httpService)
    {
        _httpService = httpService;
    }


    public async Task<List<Person>> GetPersonsAsync()
    {
        return await _httpService.Get<List<Person>>("api/person");
    }

    public async Task<Person> GetPersonAsync(int id)
    {
        return await _httpService.Get<Person>($"api/person/{id}");
    }

    public async Task AddPersonAsync(Person person)
    {
        await _httpService.Post($"api/person", person);
    }

    public async Task UpdatePersonAsync(Person person)
    {
        await _httpService.Put($"api/Person/{person.Id}", person);
    }

    public async Task DeletePersonAsync(int id)
    {
        await _httpService.Delete($"api/Person/{id}");
    }
}