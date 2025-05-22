import { useEffect, useState } from "react";
import API from "../services/api";

export default function EmployeeList() {
  const [employees, setEmployees] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchEmployees() {
      try {
        const response = await API.get("/employees");
        setEmployees(response.data);
      } catch (err) {
        setError(err.response?.data?.message || err.message);
      } finally {
        setLoading(false);
      }
    }

    fetchEmployees();
  }, []);

  if (loading) return <p>Loading employees...</p>;
  if (error) return <p className="text-red-600">Error: {error}</p>;
  if (employees.length === 0) return <p>No employees found.</p>;

  return (
    <table className="w-full border-collapse border border-gray-300">
      <thead>
        <tr>
          <th className="border border-gray-300 p-2">First Name</th>
          <th className="border border-gray-300 p-2">Middle Name</th>
          <th className="border border-gray-300 p-2">Last Name</th>
          <th className="border border-gray-300 p-2">Date of Birth</th>
          <th className="border border-gray-300 p-2">Gender</th>
          <th className="border border-gray-300 p-2">Address</th>
        </tr>
      </thead>
      <tbody>
        {employees.map((emp) => (
          <tr key={emp.id} className="hover:bg-gray-100">
            <td className="border border-gray-300 p-2">{emp.firstName}</td>
            <td className="border border-gray-300 p-2">{emp.middleName || "-"}</td>
            <td className="border border-gray-300 p-2">{emp.lastName}</td>
            <td className="border border-gray-300 p-2">{new Date(emp.dateOfBirth).toLocaleDateString()}</td>
            <td className="border border-gray-300 p-2">{emp.gender}</td>
            <td className="border border-gray-300 p-2">{emp.address}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
