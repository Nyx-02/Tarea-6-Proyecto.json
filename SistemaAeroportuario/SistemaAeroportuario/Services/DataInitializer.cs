public static class DataInitializer
{
    public static void Initialize()
    {
        new Pasajero().InitializeSampleData();
        new Tripulacion().InitializeSampleData();
        new Vuelo().InitializeSampleData();
        new Avion().InitializeSampleData();
        new Aeropuerto().InitializeSampleData();
        new Boleto().InitializeSampleData();
    }
}