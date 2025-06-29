<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
      xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="html" indent="yes" />

	<xsl:template match="/">
		<html>
			<head>
				<title>User Login Records</title>
				<style>
					table { border-collapse: collapse; width: 100%; }
					th, td { border: 1px solid #ddd; padding: 8px; }
					th { background-color: #f2f2f2; }
					.badge-success { background-color: #28a745; color: white; padding: 3px 6px; border-radius: 4px; }
					.badge-danger { background-color: #dc3545; color: white; padding: 3px 6px; border-radius: 4px; }
				</style>
			</head>
			<body>
				<div class="card mt-4">
					<div class="card-header">
						<h5>User Login Records</h5>
					</div>
					<div class="card-body">
						<div class="table-responsive">
							<table class="table">
								<thead>
									<tr>
										<th>ID</th>
										<th>
											User ID
										</th>
										<th>
											IP Address
										</th>
										<th>
											Situation
										</th>
										<th>
											Time
										</th>
									</tr>
								</thead>
								<tbody>
								
									<xsl:for-each select="ArrayOfLoginEvent/LoginEvent">
										<tr>
											<td>
												<xsl:value-of select="id"/>
											</td>
											<td>
												<xsl:value-of select="userId"/>
											</td>
											<td>
												<xsl:value-of select="ipAddress"/>
											</td>
											<td>
												<xsl:choose>
													<xsl:when test="isSuccessful='true'">
														<span class="badge-success">Successful</span>
													</xsl:when>
													<xsl:otherwise>
														<span class="badge-danger">
															Unsuccessful
														</span>
													</xsl:otherwise>
												</xsl:choose>
											</td>
											<td>
												<xsl:value-of select="timestamp"/>
											</td>
										</tr>
									</xsl:for-each>
								</tbody>
							</table>
						</div>
					</div>
				</div>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>
