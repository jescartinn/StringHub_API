using AutoMapper;
using StringHub.Models;
using StringHub.DTOs;

namespace StringHub.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Usuario mappings
            CreateMap<Usuario, UsuarioDto>();
            CreateMap<UsuarioCreateDto, Usuario>();
            CreateMap<UsuarioUpdateDto, Usuario>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Raqueta mappings
            CreateMap<Raqueta, RaquetaDto>();
            CreateMap<RaquetaCreateDto, Raqueta>();
            CreateMap<RaquetaUpdateDto, Raqueta>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Cuerda mappings
            CreateMap<Cuerda, CuerdaDto>();
            CreateMap<CuerdaCreateDto, Cuerda>();
            CreateMap<CuerdaUpdateDto, Cuerda>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Servicio mappings
            CreateMap<Servicio, ServicioDto>();
            CreateMap<ServicioCreateDto, Servicio>();
            CreateMap<ServicioUpdateDto, Servicio>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // OrdenEncordado mappings
            CreateMap<OrdenEncordado, OrdenEncordadoDto>();
            CreateMap<OrdenEncordadoCreateDto, OrdenEncordado>();
            CreateMap<OrdenEncordadoUpdateDto, OrdenEncordado>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<OrdenEncordadoEstadoUpdateDto, OrdenEncordado>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // HistorialTension mappings
            CreateMap<HistorialTension, HistorialTensionDto>();
            CreateMap<HistorialTensionCreateDto, HistorialTension>();

            // Disponibilidad mappings
            CreateMap<Disponibilidad, DisponibilidadDto>();
            CreateMap<DisponibilidadCreateDto, Disponibilidad>();
            CreateMap<DisponibilidadUpdateDto, Disponibilidad>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}