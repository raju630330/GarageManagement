import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Role {
  id: number;
  roleName: string;
}

export interface Permission {
  id: number;
  name: string;
  description?: string;
}

export interface RolePermission {
  roleId: number;
  permissionId: number;
  moduleName: string;
}

@Injectable({
  providedIn: 'root'
})
export class RolePermissionService {
  private baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // Roles
  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(`${this.baseUrl}/Roles`);
  }

  createRole(role: Role): Observable<any> {
    return this.http.post(`${this.baseUrl}/Roles`, role);
  }

  updateRole(role: Role): Observable<any> {
    return this.http.put(`${this.baseUrl}/Roles/${role.id}`, role);
  }

  deleteRole(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Roles/${id}`);
  }

  // Permissions
  getPermissions(): Observable<Permission[]> {
    return this.http.get<Permission[]>(`${this.baseUrl}/Permissions`);
  }

  createPermission(permission: Permission): Observable<any> {
    return this.http.post(`${this.baseUrl}/Permissions`, permission);
  }

  updatePermission(permission: Permission): Observable<any> {
    return this.http.put(`${this.baseUrl}/Permissions/${permission.id}`, permission);
  }

  deletePermission(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Permissions/${id}`);
  }

  // Role-Permission
  getRoleModulePermissions(roleId: number, moduleName: string): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/RolePermissions/${roleId}/${moduleName}`);
  }

  addRolePermission(rolePermission: RolePermission): Observable<any> {
    return this.http.post(`${this.baseUrl}/RolePermissions`, rolePermission);
  }

  removeRolePermission(roleId: number, permissionId: number, moduleName: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/RolePermissions?roleId=${roleId}&permissionId=${permissionId}&moduleName=${moduleName}`);
  }
}
