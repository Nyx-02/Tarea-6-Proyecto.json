public static class DataInitializer
{
    public static void Initialize()
    {
        new Vuelo().InitializeSampleData();
        new Avion().InitializeSampleData();
        new Pasajero().InitializeSampleData();
        new Boleto().InitializeSampleData();
        new Tripulacion().InitializeSampleData();
        new Aeropuerto().InitializeSampleData();
    }
}