namespace Lacuna_Dev_Admission.Requests;

public class NewJobRequest
{
    /*
     * '[GET] /api/dna/jobs'
        Header
          Authorization = 'Bearer <AccessToken>' // <AccessToken> aquired on 2.2

        Response
        {
          job?: {
            // Job id
            id: string,
            
            // Operation types ['DecodeStrand', 'EncodeStrand', 'CheckGene']
            type: string,
            
            // Strand in String format. Non-null when operation type 'EncodeStrand'
            strand?: string,

            // Strand in the Binary format Base64 encoded. Non-null when operation types 'DecodeStrand' and 'CheckGene'
            strandEncoded?: string,

            // A gene segment in the Binary format Base64 encoded. Non-null when operation type 'CheckGene'
            geneEncoded?: string
          },
          code: string, // ['Success', 'Error', 'Unauthorized']
          message?: string
        }
     */
    
}