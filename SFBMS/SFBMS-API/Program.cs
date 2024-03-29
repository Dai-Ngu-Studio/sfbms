using BusinessObject;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.Interfaces;
using SFBMS_API.Controllers;
using SFBMS_API.Services;
using SFBMS_API.Utilities;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers(o =>
{
    o.InputFormatters.Insert(o.InputFormatters.Count, new TextPlainInputFormatter());
}).AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddHostedService<BookingStatusService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFieldRepository, FieldRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingDetailRepository, BookingDetailRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<ISlotRepository, SlotRepository>();

builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Allows summary/documentation for API endpoints.
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.EnableAnnotations();
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Format: `Bearer <token>`",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

static IEdmModel GetEdmModel()
{
    var modelBuilder = new ODataConventionModelBuilder();
    modelBuilder.EnableLowerCamelCase();
    modelBuilder.EntitySet<User>("Users").EntityType.HasKey(x => x.Id);
    modelBuilder.EntitySet<Field>("Fields").EntityType.HasKey(x => x.Id);
    modelBuilder.EntitySet<Slot>("Slots").EntityType.HasKey(x => x.Id);
    modelBuilder.EntitySet<Category>("Categories").EntityType.HasKey(x => x.Id);
    modelBuilder.EntitySet<Booking>("Bookings").EntityType.HasKey(x => x.Id);
    modelBuilder.EntitySet<BookingDetail>("BookingDetails").EntityType.HasKey(x => x.Id);
    modelBuilder.EntitySet<Feedback>("Feedbacks").EntityType.HasKey(x => x.Id);
    var field = modelBuilder.EntityType<Field>();
    field.Action(nameof(FieldsController.SlotStatus))
        .ReturnsFromEntitySet<Field>("Fields")
        .Parameter<DateTimeOffset>("BookingDate").Optional();

    var fields = modelBuilder.EntityType<Field>().Collection;
    var filter = fields.Action(nameof(FieldsController.Filter));
    filter.ReturnsCollectionFromEntitySet<Field>("Fields");
    filter.CollectionParameter<int>("CategoryIDs").Optional();
    //filter.CollectionParameter<int>("BookingTimeEnums").Optional();
    return modelBuilder.GetEdmModel();
}

builder.Services.AddControllers().AddOData(option =>
option
.Select()
.Filter()
.Count()
.OrderBy()
.Expand()
.SetMaxTop(100)
.AddRouteComponents("odata", GetEdmModel()));

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.GetApplicationDefault(),
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = "https://securetoken.google.com/sfbms-48a15";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "https://securetoken.google.com/sfbms-48a15",
        ValidateAudience = true,
        ValidAudience = "sfbms-48a15",
        ValidateLifetime = true,
    };
});

builder.Services.AddDbContext<SfbmsDbContext>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<SfbmsDbContext>())
    context!.Database.Migrate();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});

app.UseCors(options =>
{
    options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});

app.UseRouting();

app.UseODataBatching();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
