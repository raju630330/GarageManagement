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

export interface RolePermissionDto {
  id?: number;
  roleId: number;
  permissionId: number;
  moduleName: string;
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

  /* ===================== ROLE – PERMISSION ===================== */
  getRoleModulePermissions(roleId: number, moduleName: string): Observable<string[]> {
    return this.http.get<string[]>(
      `${this.baseUrl}/RolePermissions/${roleId}/${moduleName}`
    );
  }

  saveRolePermission(dto: RolePermissionDto): Observable<BaseResultDto> {
    return this.http.post<BaseResultDto>(`${this.baseUrl}/RolePermissions`, dto);
  }

  removeRolePermission(roleId: number, permissionId: number, moduleName: string): Observable<BaseResultDto> {
    return this.http.delete<BaseResultDto>(
      `${this.baseUrl}/RolePermissions?roleId=${roleId}&permissionId=${permissionId}&moduleName=${moduleName}`
    );
  }

  clearRoleModulePermissions(roleId: number, moduleName: string): Observable<BaseResultDto> {
    return this.http.delete<BaseResultDto>(
      `${this.baseUrl}/RolePermissions/clear?roleId=${roleId}&moduleName=${moduleName}`
    );
  }
}
