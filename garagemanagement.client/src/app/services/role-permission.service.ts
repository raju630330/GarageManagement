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

export interface PermissionModule {
  id: number;
  name: string;
  description?: string;
}

export interface RolePermissionDto {
  id?: number;
  roleId: number;
  permissionId: number;
  permissionModuleId: number;
  moduleName?: string; // optional, for convenience
}

export interface BaseResultDto {
  id?: number;
  isSuccess: boolean;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class RolePermissionService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  /* ===================== ROLES ===================== */
  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(`${this.baseUrl}/Roles`);
  }

  saveRole(role: Role): Observable<BaseResultDto> {
    return this.http.post<BaseResultDto>(`${this.baseUrl}/Roles`, role);
  }

  deleteRole(id: number): Observable<BaseResultDto> {
    return this.http.delete<BaseResultDto>(`${this.baseUrl}/Roles/${id}`);
  }

  /* ===================== PERMISSIONS ===================== */
  getPermissions(): Observable<Permission[]> {
    return this.http.get<Permission[]>(`${this.baseUrl}/Permissions`);
  }

  savePermission(permission: Permission): Observable<BaseResultDto> {
    return this.http.post<BaseResultDto>(`${this.baseUrl}/Permissions`, permission);
  }

  deletePermission(id: number): Observable<BaseResultDto> {
    return this.http.delete<BaseResultDto>(`${this.baseUrl}/Permissions/${id}`);
  }

  /* ===================== MODULES ===================== */
  getModules(): Observable<PermissionModule[]> {
    return this.http.get<PermissionModule[]>(`${this.baseUrl}/RolePermissions/modules`);
  }

  addModule(module: PermissionModule): Observable<BaseResultDto> {
    return this.http.post<BaseResultDto>(`${this.baseUrl}/RolePermissions/module`, module);
  }

  /* ===================== ROLE – PERMISSION ===================== */
  getRolePermissions(roleId: number): Observable<RolePermissionDto[]> {
    return this.http.get<RolePermissionDto[]>(`${this.baseUrl}/RolePermissions/role/${roleId}`);
  }

  getRoleModulePermissions(roleId: number, moduleId: number): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/RolePermissions/role/${roleId}/module/${moduleId}`);
  }

  saveRolePermission(dto: RolePermissionDto): Observable<BaseResultDto> {
    return this.http.post<BaseResultDto>(`${this.baseUrl}/RolePermissions/permission`, dto);
  }

  removeRolePermission(roleId: number, permissionId: number, moduleId: number): Observable<BaseResultDto> {
    return this.http.delete<BaseResultDto>(
      `${this.baseUrl}/RolePermissions/permission?roleId=${roleId}&permissionId=${permissionId}&moduleId=${moduleId}`
    );
  }

  clearRoleModulePermissions(roleId: number, moduleId: number): Observable<BaseResultDto> {
    return this.http.delete<BaseResultDto>(
      `${this.baseUrl}/RolePermissions/role/${roleId}/module/${moduleId}/clear`
    );
  }

  hasPermission(roleId: number, moduleId: number, permissionName: string): Observable<boolean> {
    return this.http.get<boolean>(
      `${this.baseUrl}/RolePermissions/check?roleId=${roleId}&moduleId=${moduleId}&permissionName=${permissionName}`
    );
  }

  /* ===================== HELPERS ===================== */

  assignDefaultPermission(
    roleId: number,
    moduleId: number
  ): Observable<BaseResultDto> {
    return this.http.post<BaseResultDto>(
      `${this.baseUrl}/RolePermissions/assign-default`,
      { roleId, moduleId }
    );
  }

  clearAllRolePermissions(roleId: number): Observable<BaseResultDto> {
    return this.http.delete<BaseResultDto>(
      `${this.baseUrl}/RolePermissions/role/${roleId}/clear`
    );
  }
}
