namespace CitasMedicasFront.Helpers
{
    public static class ApiUrls
    {
        public static string Base => "https://localhost:44323/api/";
        
        public static string Roles => $"{Base}Roles";
        public static string Pacientes => $"{Base}Pacientes";
        public static string Medicos => $"{Base}Medicos";
        public static string Consultorios => $"{Base}Consultorios";
        public static string EstatusCita => $"{Base}EstatusCita";
        public static string Citas => $"{Base}Citas";
        public static string Personal => $"{Base}Personal";
        public static string Usuarios => $"{Base}Usuarios";
        public static string Departamentos => $"{Base}Departamentos";
    }
}
