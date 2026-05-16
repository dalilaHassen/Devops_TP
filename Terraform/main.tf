terraform {
  required_providers {
    docker = {
      source = "kreuzwerker/docker"
    }
  }
}

provider "docker" {}

# Image Ubuntu
resource "docker_image" "ubuntu" {
  name = "ubuntu:latest"
}

# Container (simule une VM)
resource "docker_container" "server" {
  name = "devops-server"
  image = docker_image.ubuntu.name
  tty = true
  command = ["sleep", "3600"]
  ports {
    internal = 80
    external = 6000
  }
}

# ===== NOUVEAU CODE POUR L'API .NET =====
# Image de votre API .NET
resource "docker_image" "incidents_api" {
  name = "dalila844/incidents-api:1.0"
}

# Conteneur de votre API .NET
resource "docker_container" "incidents_api_container" {
  name  = "incidents-api-net"
  image = docker_image.incidents_api.name
  
  ports {
    internal = 80
    external = 8080
  }
  
  restart = "always"
}

# Afficher l'URL d'accès
output "api_endpoint" {
  value = "http://localhost:8080/swagger"
  description = "URL pour accéder à l'API Swagger"
}